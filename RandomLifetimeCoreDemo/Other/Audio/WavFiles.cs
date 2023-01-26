namespace RandomLifetimeCoreDemo.Other.Audio;

public sealed class WavFiles
{
    public readonly IReadOnlyList<string> Files;

    public WavFiles(string directory)
    {
        Files = Directory
            .EnumerateFiles(directory)
            .Where(f => f.EndsWith(".wav"))
            .ToArray();
    }
    
    public void PlayRandom(Random random)
    {
        var file = Files[random.Next(Files.Count)];
        // TODO: Make an audio thread. Disposing player stops audio
        using var player = new WavPlayer(file);
        player.Play();
    }
}