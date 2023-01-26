using System.Diagnostics;
using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other.Audio;
using SimpleLogger;

namespace RandomLifetimeCoreDemo;

public static class Program
{
    public static void Main()
    {
        Logger.SharedConsoleLogger.Redirect(Console.Out, true);
        Console.WriteLine($"PID: {Environment.ProcessId}");
        var deathChecker = new ParallelLifetimeDeathChecker(TimeSpan.FromMilliseconds(500));
        var fireworkSoundDir = $"{new DirectoryInfo(Directory.GetCurrentDirectory())
            .Parent!
            .Parent!
            .Parent!
            .FullName}\\FireworkSoundEffects";
        Console.WriteLine($"Sound effect dir abs path: {fireworkSoundDir}");
        
        var sounds =
            new WavFiles(fireworkSoundDir);
        
        const int fireworksToExplode = 60;
        const int secondsForFireworksToExplode = 60;
        for (var i = 0; i < fireworksToExplode; i++)
        {
            var fw = Firework.Random(Random.Shared, TimeSpan.FromSeconds(secondsForFireworksToExplode));
            fw.OnDeath += () => sounds.PlayRandom(Random.Shared);
            deathChecker.BeginWatching(fw);
        }

        deathChecker.StartWatchThread();
        Thread.Sleep(TimeSpan.FromSeconds(secondsForFireworksToExplode) + TimeSpan.FromMilliseconds(sounds.MsBeforeDisposingPlayer));
        deathChecker.StopWatchThread();
    }
}