using System.Collections.Concurrent;
using System.Diagnostics;
using SimpleLogger;

namespace RandomLifetimeCoreDemo.Living;

/// <summary>
/// A class housing a separate thread that
/// monitors all instances being watched in
/// a HashSet. This class is thread-safe
/// through a ConcurrentQueue of predefined
/// actions that is emptied after
/// each enumeration of the monitored
/// HashSet.
/// </summary>
public sealed class ParallelLifetimeDeathChecker
{
    /// <summary>
    /// The millisecond interval the thread will sleep
    /// during to prevent high CPU usage. This can
    /// be changed while the thread is started.
    ///
    /// The higher the interval, the more inaccurate
    /// the time of death will be caught for any
    /// instance. If this is set too high, a side effect
    /// may be instance deaths being caught in batches.
    /// </summary>
    private readonly HashSet<LivingInstance> _instances;
    private readonly ConcurrentQueue<Action> _queuedSetActions;
    private readonly Thread _thread;
    private volatile bool _aliveThread;
    private readonly Semaphore _monitorGate;
    private readonly Timer _openGateTimer;

    public ParallelLifetimeDeathChecker(TimeSpan minIntervalBetweenChecks)
    {
        _instances = new();
        _queuedSetActions = new();
        _monitorGate = new(1, 1, "Death Monitor Gate");
        _openGateTimer = new Timer
            (_ =>
            {
                try
                {
                    _monitorGate.Release();
                }
                catch (SemaphoreFullException)
                {
                    // Ignore full semaphore exceptions
                    // in case monitor did not complete in time
                }
            }, null, TimeSpan.Zero, minIntervalBetweenChecks);
        _thread = new(Monitor);
        _aliveThread = true;
    }
    
    /// <summary>
    /// The minimum time between checking for deaths within the
    /// LivingInstance container and completing the
    /// predefined actions set by the class. If completing a
    /// current enumeration takes longer than 100ms, the
    /// thread will not wait to begin the next enumeration. If
    /// it takes less than 100ms, the thread will wait until
    /// the 100th millisecond passes from the beginning of
    /// the enumeration.
    /// </summary>
    /// <param name="interval">The new time to wait at least between each enumeration</param>
    public void ChangeInterval(TimeSpan interval)
    {
        Logger.SharedConsoleLogger.Log("Changing a death checker's minimum enumeration interval");
        _openGateTimer.Change(TimeSpan.Zero, interval);
    }

    /// <summary>
    /// Queues the argument instance to be added
    /// into the monitored HashSet. This is
    /// done after the next enumeration of the
    /// HashSet completes.
    /// </summary>
    /// <param name="instance">The instance to be added to the HashSet</param>
    public void BeginWatching(LivingInstance instance)
    {
        Logger.SharedConsoleLogger.Log("Adding an object to a death checker watch pool");
        Debug.Assert(instance != null);

        void AddAction()
        {
            _instances.Add(instance);
        }

        _queuedSetActions.Enqueue(AddAction);
    }

    /// <summary>
    /// Queues the argument instance to be
    /// removed from the monitored HashSet.
    /// This is done after the next enumeration
    /// of the HashSet completes.
    /// </summary>
    /// <param name="instance">The instance to be removed from the HashSet</param>
    public void StopWatching(LivingInstance instance)
    {
        Debug.Assert(instance != null);

        void RemoveAction()
        {
            _instances.Remove(instance);
        }

        _queuedSetActions.Enqueue(RemoveAction);
    }

    /// <summary>
    /// Begins the thread that monitors all instances
    /// being watched. This is recommended to be called
    /// as soon as an instance of this class is created.
    /// </summary>
    public void StartWatchThread()
    {
        _thread.Start();
    }

    /// <summary>
    /// Ends the thread monitoring all instances being
    /// watched in the HashSet. The current enumeration
    /// and queue progress is completed first to safely
    /// end the thread.
    /// </summary>
    public void StopWatchThread()
    {
        Logger.SharedConsoleLogger.Log("Stopping a death checker thread");
        _aliveThread = false;
    }
    
    /// <summary>
    /// The monitoring action run
    /// in the thread
    /// </summary>
    private void Monitor()
    {
        while (_aliveThread)
        {
            var time = DateTime.Now;
            Logger.SharedConsoleLogger.Log("Beginning enumeration in a death checker instance");
            foreach (var instance in _instances)
            {
                if (time < instance.PlannedDeathTime)
                    continue;

                instance.OnDeath?.Invoke();
                StopWatching(instance);
            }

            while (!_queuedSetActions.IsEmpty)
            {
                _queuedSetActions.TryDequeue(out var result);
                result?.Invoke();
            }

            _monitorGate.WaitOne();
        }
    }
}