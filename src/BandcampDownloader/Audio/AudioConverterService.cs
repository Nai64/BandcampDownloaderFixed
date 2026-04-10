using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BandcampDownloader.Settings;
using NAudio.MediaFoundation;
using NAudio.Wave;

namespace BandcampDownloader.Audio;

internal interface IAudioConverterService
{
    /// <summary>
    /// Converts the MP3 file to the specified bitrate (kbps).
    /// Returns the path to the converted file (same path, overwrites original).
    /// </summary>
    void ConvertToBitrate(string filePath, int targetBitrateKbps, CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets the list of supported bitrates for MP3 encoding.
    /// </summary>
    IReadOnlyList<int> GetSupportedBitrates();
}

internal sealed class AudioConverterService : IAudioConverterService
{
    private readonly IUserSettings _userSettings;
    // Standard MP3 bitrates supported by most encoders
    private static readonly IReadOnlyList<int> StandardMp3Bitrates = new List<int> { 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320 };

    public AudioConverterService(ISettingsService settingsService)
    {
        _userSettings = settingsService.GetUserSettings();
        MediaFoundationApi.Startup();
    }
    
    public IReadOnlyList<int> GetSupportedBitrates()
    {
        // Return standard MP3 bitrates (Media Foundation's MP3 encoder supports these)
        return StandardMp3Bitrates;
    }

    public void ConvertToBitrate(string filePath, int targetBitrateKbps, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            return;
        }

        // Create temp files for conversion
        var tempWav = filePath + ".temp.wav";
        var tempMp3 = filePath + ".temp.mp3";

        try
        {
            // Step 1: Decode MP3 to WAV (keeping original sample rate)
            using (var reader = new Mp3FileReader(filePath))
            {
                WaveFileWriter.CreateWaveFile(tempWav, reader);
            }

            // Step 2: Encode WAV to MP3 at target bitrate
            var targetBitrate = targetBitrateKbps * 1000; // Convert kbps to bps
            
            using (var reader = new WaveFileReader(tempWav))
            {
                // Encode to MP3 - Media Foundation will use closest supported bitrate
                MediaFoundationEncoder.EncodeToMp3(reader, tempMp3, targetBitrate);
            }

            // Log conversion results
            var originalSize = new FileInfo(filePath).Length;
            var convertedSize = new FileInfo(tempMp3).Length;
            var expectedRatio = targetBitrateKbps / 128.0;
            var actualRatio = (double)convertedSize / originalSize;
            var estimatedActualBitrate = (int)(actualRatio * 128);
            System.Diagnostics.Debug.WriteLine($"[AudioConverter] {originalSize / (1024 * 1024.0):F1} MB -> {convertedSize / (1024 * 1024.0):F1} MB (requested: {targetBitrateKbps} kbps, actual: ~{estimatedActualBitrate} kbps, ratio: {actualRatio:F2}, expected: {expectedRatio:F2})");
            
            // Warn if the actual bitrate differs significantly from requested
            if (Math.Abs(actualRatio - expectedRatio) > 0.15)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioConverter] WARNING: Media Foundation encoder ignored requested bitrate ({targetBitrateKbps} kbps). Output is ~{estimatedActualBitrate} kbps instead.");
            }

            // Step 3: Replace original with converted file
            File.Delete(filePath);
            File.Move(tempMp3, filePath);
        }
        catch
        {
            // Clean up temp files if they exist
            if (File.Exists(tempWav)) File.Delete(tempWav);
            if (File.Exists(tempMp3)) File.Delete(tempMp3);
            throw;
        }
        finally
        {
            // Clean up WAV temp file
            if (File.Exists(tempWav)) File.Delete(tempWav);
        }
    }
}
