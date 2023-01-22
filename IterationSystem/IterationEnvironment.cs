namespace IterationSystem;

public class IterationEnvironment
{
    public TimeSpan RunTime { get; set; }
    public Action? MillisecondIterationActions;
    public volatile bool ContinueLooping;

    public IterationEnvironment(TimeSpan timeUntilStop, params Action[] actions)
    {
        RunTime = timeUntilStop;
        ContinueLooping = true;
        foreach (var action in actions)
        {
            MillisecondIterationActions += action;
        }
    }

    private void IterationAction()
    {
        if (MillisecondIterationActions == null)
        {
            Console.Error.WriteLine("WARNING: No actions to complete in iteration environment");
            Console.Error.WriteLine("Starting empty iteration loop...");
            while (ContinueLooping) ;
            return;
        }
        
        while (ContinueLooping)
        {
            MillisecondIterationActions.Invoke();
        }
    }

    public Task BeginThread()
    {
        var thread = new Thread(IterationAction);
        thread.Start();
        return Task.Delay(RunTime).ContinueWith(_ => ContinueLooping = false);
    }

    public void BeginBlocking()
    {
        Task.Delay(RunTime).ContinueWith(_ => ContinueLooping = false);
        IterationAction();
    }
}