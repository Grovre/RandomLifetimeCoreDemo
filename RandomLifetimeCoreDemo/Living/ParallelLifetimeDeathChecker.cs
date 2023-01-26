using System.Collections.Concurrent;
using System.Diagnostics;

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
    public int MonitorMsInterval { get; set; } = 100;
    private readonly HashSet<LivingInstance> _instances;
    private readonly ConcurrentQueue<Action> _queuedSetActions;
    private readonly Thread _thread;
    private volatile bool _aliveThread;

    public ParallelLifetimeDeathChecker()
    {
        _instances = new();
        _queuedSetActions = new();
        _thread = new(Monitor);
        _aliveThread = true;
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
            Thread.Sleep(MonitorMsInterval);
            var time = DateTime.Now;
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
        }
    }
}