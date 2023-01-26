using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other.Audio;

namespace RandomLifetimeCoreDemo;

public static class Program
{
    public static void Main()
    {
        var deathChecker = new ParallelLifetimeDeathChecker();
        var fireworkSoundDir = $"{new DirectoryInfo(Directory.GetCurrentDirectory())
            .Parent!
            .Parent!
            .Parent!
            .FullName}\\FireworkSoundEffects";
        Console.WriteLine(fireworkSoundDir);
        
        var sounds =
            new WavFiles(fireworkSoundDir);

        const int fireworksToExplode = 100;
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