using SimpleLogger;

namespace RandomLifetimeCoreDemo.Other.Audio;

/// <summary>
/// A class that provides simplification to
/// using multiple .wav files. Creates a
/// player every time an audio file is
/// fetched and disposes of it automatically
/// after a constant amount of time. It is
/// recommended to change this after
/// instantiation to something at least
/// as long as the longest .wav file
/// in the directory given.
/// </summary>
public sealed class WavFiles
{
    /// <summary>
    /// Time before the player object is disposed.
    /// Recommended to be at least as long as
    /// the longest .wav file in the provided
    /// directory.
    /// </summary>
    public int MsBeforeDisposingPlayer { get; set; } = 7_500;
    public IReadOnlyList<string> Files { get; }
    public string DirectoryPath { get; set; }

    public WavFiles(string directory)
    {
        DirectoryPath = directory;
        Files = Directory
            .EnumerateFiles(directory)
            .Where(f => f.EndsWith(".wav"))
            .ToArray();
    }
    
    /// <summary>
    /// Plays a random .wav file from the given directory.
    /// </summary>
    /// <param name="random">The random object used to get the random .wav file</param>
    public void PlayRandom(Random random)
    {
        Logger.SharedConsoleLogger.Log("Playing a random sound");
        var file = Files[random.Next(Files.Count)];
        var player = new WavPlayer(file);
        player.Play();
        Task.Delay(MsBeforeDisposingPlayer).ContinueWith(_ => player.Dispose());
    }
}