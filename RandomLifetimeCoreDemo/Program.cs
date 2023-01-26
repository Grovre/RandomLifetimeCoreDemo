using System.Text;
using IterationSystem;
using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other;

namespace RandomLifetimeCoreDemo;

public static class Program
{
    public static void Main()
    {
        var deathChecker = new ParallelLifetimeDeathChecker();
        
        for (var i = 0; i < 20; i++)
        {
            var fw = Firework.Random(Random.Shared, TimeSpan.FromSeconds(19));
            deathChecker.BeginWatching(fw);
        }

        deathChecker.StartWatchThread();
        Thread.Sleep(TimeSpan.FromSeconds(20));
        deathChecker.StopWatchThread();
    }
}