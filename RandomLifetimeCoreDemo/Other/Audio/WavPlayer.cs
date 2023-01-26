using NAudio.Wave;
using SimpleLogger;

namespace RandomLifetimeCoreDemo.Other.Audio;

/// <summary>
/// Provides an abstraction to using NAudio
/// to play .wav files using an
/// AudioFileReader and WaveOutEvent.
/// </summary>
public sealed class WavPlayer : IDisposable
{
    /// <summary>
    /// The path towards a .wav file
    /// that the underlying streams are
    /// based off of.
    /// </summary>
    public readonly string WavPath;
    private readonly AudioFileReader _audioFileReader;
    private readonly WaveOutEvent _waveOutEvent;

    /// <summary>
    /// Uses the path argument to create
    /// the underlying streams with
    /// in order to play the audio
    /// in the future.
    /// </summary>
    /// <param name="wavPath"></param>
    public WavPlayer(string wavPath)
    {
        WavPath = wavPath;
        _audioFileReader = new(wavPath);
        _waveOutEvent = new();
        _waveOutEvent.Init(_audioFileReader);
    }

    /// <summary>
    /// Plays the audio file from
    /// the beginning and at 100%
    /// volume.
    /// </summary>
    public void Play()
    {
        Logger.SharedConsoleLogger.Log($"Playing sound {_audioFileReader.FileName}");
        _audioFileReader.Position = 0L;
        _audioFileReader.Volume = 1F;
        _waveOutEvent.Play();
    }

    /// <summary>
    /// Disposes all objects associated
    /// with this instance.
    /// </summary>
    public void Dispose()
    {
        Logger.SharedConsoleLogger.Log("Disposing a WavPlayer");
        _audioFileReader.Dispose();
        _waveOutEvent.Dispose();
    }
}