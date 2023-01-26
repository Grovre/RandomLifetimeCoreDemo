namespace RandomLifetimeCoreDemo.Other.Audio;

public sealed class WavFiles
{
    public const int MsBeforeDisposingPlayer = 7_500;
    public readonly IReadOnlyList<string> Files;
    public readonly string DirectoryPath;

    public WavFiles(string directory)
    {
        DirectoryPath = directory;
        Files = Directory
            .EnumerateFiles(directory)
            .Where(f => f.EndsWith(".wav"))
            .ToArray();
    }
    
    public void PlayRandom(Random random)
    {
        var file = Files[random.Next(Files.Count)];
        var player = new WavPlayer(file);
        player.Play();
        Task.Delay(MsBeforeDisposingPlayer).ContinueWith(_ => player.Dispose());
    }
}