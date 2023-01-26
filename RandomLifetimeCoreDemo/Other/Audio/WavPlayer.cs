using NAudio.Wave;

namespace RandomLifetimeCoreDemo.Other.Audio;

public sealed class WavPlayer : IDisposable
{
    public readonly string WavPath;
    private readonly AudioFileReader _audioFileReader;
    private readonly WaveOutEvent _waveOutEvent;

    public WavPlayer(string wavPath)
    {
        WavPath = wavPath;
        _audioFileReader = new(wavPath);
        _waveOutEvent = new();
        _waveOutEvent.Init(_audioFileReader);
    }

    public void Play()
    {
        _audioFileReader.Position = 0L;
        _audioFileReader.Volume = 1F;
        _waveOutEvent.Play();
    }

    public void Dispose()
    {
        _audioFileReader.Dispose();
        _waveOutEvent.Dispose();
    }
}