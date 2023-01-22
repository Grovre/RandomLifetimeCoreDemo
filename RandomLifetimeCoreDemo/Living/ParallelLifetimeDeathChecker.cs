using System.Collections.Concurrent;
using System.Diagnostics;

namespace RandomLifetimeCoreDemo.Living;

public sealed class ParallelLifetimeDeathChecker
{
    private readonly HashSet<LivingInstance> _instances;
    private readonly ConcurrentQueue<Action> _queuedSetActions;
    private readonly Thread _thread;

    public ParallelLifetimeDeathChecker()
    {
        _instances = new();
        _queuedSetActions = new();
        _thread = new(Monitor);
    }

    public void BeginWatching(LivingInstance instance)
    {
        Debug.Assert(instance != null);

        void AddAction()
        {
            _instances.Add(instance);
        }

        _queuedSetActions.Enqueue(AddAction);
    }

    public void StopWatching(LivingInstance instance)
    {
        Debug.Assert(instance != null);

        void RemoveAction()
        {
            _instances.Remove(instance);
        }

        _queuedSetActions.Enqueue(RemoveAction);
    }

    public void StartWatchThread()
    {
        _thread.Start();
    }

    private void Monitor()
    {
        while (true)
        {
            var time = DateTime.Now.Millisecond;
            foreach (var instance in _instances)
            {
                if (time > instance.PlannedDeathTime)
                    continue;
                
                instance.WhenPastExpectedDeath();
                if (instance.RemoveFromWatchAfterDeathTime)
                {
                    StopWatching(instance);
                }
            }

            while (!_queuedSetActions.IsEmpty)
            {
                var success = _queuedSetActions.TryDequeue(out var result);
                if (!success || result == null)
                    continue;
                
                result.Invoke();
            }
        }
    }
}