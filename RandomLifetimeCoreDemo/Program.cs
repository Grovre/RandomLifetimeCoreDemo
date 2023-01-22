using System.Text;
using IterationSystem;

namespace RandomLifetimeCoreDemo;

public static class Program
{
    public static async Task Main()
    {
        uint iter = 0;
        var iterEnv = new IterationEnvironment(TimeSpan.FromSeconds(10));
        iterEnv.MillisecondIterationActions += () => iter += 1u;
        Console.WriteLine("Starting!");
        iterEnv.BeginBlocking();
        Console.WriteLine($"Done! {iter} iterations completed");
    }
}