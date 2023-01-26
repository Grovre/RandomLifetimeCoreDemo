namespace IterationSystem;

public class IterationEnvironment
{
    public Action? MillisecondIterationActions { get; set; }
    public bool ContinueLooping { get; set; }
    public TimeSpan IterationInterval { get; set; }
    private readonly SemaphoreSlim _envGate;
    private readonly Timer _openGateTimer;

    public IterationEnvironment(TimeSpan iterationInterval, params Action[] actions)
    {
        IterationInterval = iterationInterval;
        _envGate = new(1, 1);
        ContinueLooping = true;
        foreach (var action in actions)
        {
            MillisecondIterationActions += action;
        }

        _openGateTimer = new(_ =>
        {
            try
            {
                _envGate.Release();
            }
            catch (SemaphoreFullException)
            {
                // Doesn't need to crash a program,
                // works as intended
            }
        }, null, TimeSpan.Zero, iterationInterval);
    }

    private void IterationAction()
    {
        if (MillisecondIterationActions == null)
        {
            Console.Error.WriteLine("WARNING: No actions to complete in iteration environment");
            Console.Error.WriteLine("Starting empty iteration loop...");
            // ReSharper disable once EmptyEmbeddedStatement
            while (ContinueLooping)
            {
                _envGate.Wait();
            }

            return;
        }
        
        while (ContinueLooping)
        {
            MillisecondIterationActions.Invoke();
            _envGate.Wait();
        }
    }

    public void ChangeInterval(TimeSpan interval)
    {
        IterationInterval = interval;
        _openGateTimer.Change(TimeSpan.Zero, interval);
    }

    public void BeginThread()
    {
        var thread = new Thread(IterationAction);
        thread.Start();
    }

    /// <summary>
    /// Use with caution. *Will not exit on its own.
    /// </summary>
    public void BeginBlocking()
    {
        IterationAction();
    }
}