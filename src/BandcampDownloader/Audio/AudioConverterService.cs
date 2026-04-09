using System.IO;
using System.Threading;
using BandcampDownloader.Settings;
using NAudio.MediaFoundation;
using NAudio.Wave;

namespace BandcampDownloader.Audio;

internal interface IAudioConverterService
{
    /// <summary>
    /// Converts the MP3 file to the specified sample rate.
    /// Returns the path to the converted file (same path, overwrites original).
    /// </summary>
    void ConvertToSampleRate(string filePath, int targetSampleRate, CancellationToken cancellationToken);
}

internal sealed class AudioConverterService : IAudioConverterService
{
    private readonly IUserSettings _userSettings;

    public AudioConverterService(ISettingsService settingsService)
    {
        _userSettings = settingsService.GetUserSettings();
        MediaFoundationApi.Startup();
    }

    public void ConvertToSampleRate(string filePath, int targetSampleRate, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            return;
        }

        // Create temp file for conversion
        var tempFile = filePath + ".tmp.mp3";

        try
        {
            using (var reader = new Mp3FileReader(filePath))
            {
                var outFormat = new WaveFormat(targetSampleRate, reader.WaveFormat.Channels);
                using (var resampler = new MediaFoundationResampler(reader, outFormat))
                {
                    WaveFileWriter.CreateWaveFile(tempFile + ".wav", resampler);
                }
            }

            // Convert WAV to MP3
            using (var reader = new WaveFileReader(tempFile + ".wav"))
            {
                MediaFoundationEncoder.EncodeToMp3(reader, tempFile, 128000);
            }

            // Replace original with converted file
            File.Delete(filePath);
            File.Move(tempFile, filePath);
        }
        catch
        {
            // Clean up temp files if they exist
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            if (File.Exists(tempFile + ".wav"))
            {
                File.Delete(tempFile + ".wav");
            }
            throw;
        }
        finally
        {
            // Clean up WAV temp file
            if (File.Exists(tempFile + ".wav"))
            {
                File.Delete(tempFile + ".wav");
            }
        }
    }
}
