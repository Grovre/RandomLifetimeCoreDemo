namespace SimpleLogger;

public sealed class Logger : IDisposable
{
    public static readonly Logger SharedConsoleLogger = new(Console.Out);
    
    private TextWriter _outWriter;
    public bool UseUtc { get; set; }
    public bool Active { get; set; }

    public Logger(TextWriter logWriter, bool active = true, bool useUtc = true)
    {
        _outWriter = logWriter;
        UseUtc = useUtc;
        _outWriter.WriteLine($"{Now()} Logger started");
        Active = active;
    }

    private string Now() => UseUtc ? $"[{DateTime.UtcNow}]: " : $"[{DateTime.Now}]: ";

    public void Log(string message)
    {
        if (!Active)
            return;
        
        _outWriter.WriteLine($"{Now()}{message}");
    }

    public void Redirect(TextWriter logWriter, bool disposePrevious)
    {
        if (disposePrevious)
        {
            _outWriter.Dispose();
        }

        _outWriter = logWriter;
    }

    public void Dispose()
    {
        Log("Disposing a logger");
        _outWriter.Dispose();
        GC.SuppressFinalize(this);
    }
}