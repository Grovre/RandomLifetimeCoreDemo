using System.Diagnostics;
using IterationSystem;
using RandomLifetimeCoreDemo.Living;
using RandomLifetimeCoreDemo.Other.Audio;
using RandomLifetimeCoreDemo.Other.Helpers;
using SimpleLogger;

namespace RandomLifetimeCoreDemo;

public static class Program
{
    public static void Main()
    {
        // Magic numbers unmagicked
        var longestTimeUntilFireworkExplosion = TimeSpan.FromSeconds(5);
        var timeUntilFastExplosions = TimeSpan.FromSeconds(15);
        var fastExplosionLength = TimeSpan.FromSeconds(10);
        var iterationEnvInterval = TimeSpan.FromMilliseconds(1000);
        var deathCheckerInterval = TimeSpan.FromMilliseconds(250);
        
        // Firework sounds
        var fireworkSoundDir = $"{new DirectoryInfo(Directory.GetCurrentDirectory())
            .Parent!
            .Parent!
            .Parent!
            .FullName}\\FireworkSoundEffects";
        Console.WriteLine($"Sound effect dir abs path: {fireworkSoundDir}");
        var sounds =
            new WavFiles(fireworkSoundDir);
        
        // Logger and visible startup actions
        Logger.SharedConsoleLogger.Redirect(Console.Out, true);
        Console.WriteLine($"PID: {Environment.ProcessId}");
        var deathChecker = new ParallelLifetimeDeathChecker(deathCheckerInterval);

        // Iteration environment setup
        var random = new Random();
        var iterEnv = new IterationEnvironment(iterationEnvInterval);
        // Iter env: adding log and firework generator action
        iterEnv.MillisecondIterationActions += 
            () => Logger.SharedConsoleLogger.Log("Next env iteration beginning");
        iterEnv.MillisecondIterationActions += () =>
        {
            if (!random.NextBool(2, 3)) return;
            var randomFirework = Firework.Random(random, longestTimeUntilFireworkExplosion);
            randomFirework.OnDeath += () => sounds.PlayRandom(random);
            deathChecker.BeginWatching(randomFirework);
        };
        
        // Controls the speed and timings
        deathChecker.StartWatchThread();
        iterEnv.BeginThread();
        Task.Delay(timeUntilFastExplosions).Wait();
        iterEnv.ChangeInterval(TimeSpan.FromMilliseconds(150));
        Task.Delay(fastExplosionLength).Wait();
        iterEnv.ContinueLooping = false;
        deathChecker.StopWatchThread();
        Task.Delay(sounds.MsBeforeDisposingPlayer).Wait(); // For not ending sounds immediately
    }
}