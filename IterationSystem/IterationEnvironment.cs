namespace IterationSystem;

public class IterationEnvironment
{
    public TimeSpan RunTime { get; set; }
    public Action<int>? MillisecondIterationActions;
}