using System.Text;
using IterationSystem;
using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other;
using RandomLifetimeCoreDemo.Other.Audio;

namespace RandomLifetimeCoreDemo;

public static class Program
{
    public static void Main_O()
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

    public static void Main()
    {
        var player =
            new WavPlayer(
                @"C:\Users\lando\RiderProjects\RandomLifetimeCoreDemo\RandomLifetimeCoreDemo\FireworkSoundEffects\street-firework.wav");
        player.Play();
        Thread.Sleep(5_000);
    }
}