using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BandcampDownloader.Helpers;
using BandcampDownloader.Model;
using BandcampDownloader.Net;
using BandcampDownloader.Settings;
using NLog;

namespace BandcampDownloader.Bandcamp.Download;

internal interface ITrackFileService
{
    Task<IReadOnlyCollection<TrackFile>> GetFilesToDownloadAsync(IReadOnlyCollection<Album> albums, CancellationToken cancellationToken);
    event DownloadProgressChangedEventHandler DownloadProgressChanged;
}

internal sealed class TrackFileService : ITrackFileService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IHttpService _httpService;
    private readonly IResilienceService _resilienceService;
    private readonly IUserSettings _userSettings;
    public event DownloadProgressChangedEventHandler DownloadProgressChanged;

    public TrackFileService(IHttpService httpService, IResilienceService resilienceService, ISettingsService settingsService)
    {
        _httpService = httpService;
        _resilienceService = resilienceService;
        _userSettings = settingsService.GetUserSettings();
    }

    public async Task<IReadOnlyCollection<TrackFile>> GetFilesToDownloadAsync(IReadOnlyCollection<Album> albums, CancellationToken cancellationToken)
    {
        var files = new List<TrackFile>();
        var filesLock = new Lock();

        // Calculate bitrate adjustment factor if force bitrate is enabled
        double bitrateAdjustmentFactor = 1.0;
        _logger.Info($"ForceBitrate setting: {_userSettings.ForceBitrate}, Bitrate setting: {_userSettings.Bitrate}");
        if (_userSettings.ForceBitrate && _userSettings.Bitrate > 0)
        {
            // Bandcamp streams are 128 kbps
            bitrateAdjustmentFactor = (double)_userSettings.Bitrate / 128.0;
            _logger.Info($"Bitrate adjustment factor: {bitrateAdjustmentFactor} (converting to {_userSettings.Bitrate} kbps)");
            DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Bitrate conversion enabled: adjusting file sizes to {_userSettings.Bitrate} kbps (factor: {bitrateAdjustmentFactor:F2})", DownloadProgressChangedLevel.Info));
        }

        foreach (var album in albums)
        {
            cancellationToken.ThrowIfCancellationRequested();

            DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Computing size for album \"{album.Title}\"...", DownloadProgressChangedLevel.Info));

            // Artwork
            if ((_userSettings.SaveCoverArtInTags || _userSettings.SaveCoverArtInFolder) && album.HasArtwork)
            {
                if (_userSettings.RetrieveFilesSize)
                {
                    var size = await GetFileSizeAsync(album.ArtworkUrl, album.Title, FileType.Artwork, cancellationToken).ConfigureAwait(false);
                    var trackFile = new TrackFile(album.ArtworkUrl, 0, size);
                    files.Add(trackFile);
                }
                else
                {
                    var trackFile = new TrackFile(album.ArtworkUrl, 0, 0);
                    files.Add(trackFile);
                }
            }

            // Tracks
            if (_userSettings.RetrieveFilesSize)
            {
                var parallelOptions = new ParallelOptions
                {
                    CancellationToken = cancellationToken,
                    MaxDegreeOfParallelism = _userSettings.MaxConcurrentTracksDownloads, // Limit the number of HTTP requests
                };

                await Parallel.ForEachAsync(
                    album.Tracks,
                    parallelOptions,
                    async (track, ct) =>
                    {
                        var size = await GetFileSizeAsync(track.Mp3Url, track.Title, FileType.Track, ct).ConfigureAwait(false);
                        
                        // Calculate adjusted size based on bitrate conversion
                        long adjustedSize = size;
                        if (bitrateAdjustmentFactor != 1.0)
                        {
                            adjustedSize = (long)(size * bitrateAdjustmentFactor);
                            _logger.Debug($"Track \"{track.Title}\" size adjusted from {size} to {adjustedSize} (factor: {bitrateAdjustmentFactor})");
                        }
                        
                        var trackFile = new TrackFile(track.Mp3Url, 0, size, adjustedSize);

                        lock (filesLock)
                        {
                            files.Add(trackFile);
                        }
                    }).ConfigureAwait(false);
            }
            else
            {
                foreach (var track in album.Tracks)
                {
                    var trackFile = new TrackFile(track.Mp3Url, 0, 0);
                    files.Add(trackFile);
                }
            }
        }

        return files;
    }

    private async Task<long> GetFileSizeAsync(string url, string titleForLog, FileType fileType, CancellationToken cancellationToken)
    {
        long size = 0;
        bool sizeRetrieved;
        var tries = 0;

        var fileTypeForLog = fileType switch
        {
            FileType.Artwork => "cover art file for album",
            FileType.Track => "MP3 file for the track",
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null),
        };

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                size = await _httpService.GetFileSizeAsync(url, cancellationToken).ConfigureAwait(false);
                sizeRetrieved = true;
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Retrieved the size of the {fileTypeForLog} \"{titleForLog}\"", DownloadProgressChangedLevel.VerboseInfo));
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.Error(ex);

                sizeRetrieved = false;
                if (tries + 1 < _userSettings.DownloadMaxTries)
                {
                    DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Failed to retrieve the size of the {fileTypeForLog} \"{titleForLog}\". Try {tries + 1} of {_userSettings.DownloadMaxTries}", DownloadProgressChangedLevel.Warning));
                }
                else
                {
                    DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Failed to retrieve the size of the {fileTypeForLog} \"{titleForLog}\". Hit max retries of {_userSettings.DownloadMaxTries}. Progress update may be wrong.", DownloadProgressChangedLevel.Error));
                }
            }

            tries++;
            if (!sizeRetrieved && tries < _userSettings.DownloadMaxTries)
            {
                await _resilienceService.WaitForCooldownAsync(tries, cancellationToken).ConfigureAwait(false);
            }
        } while (!sizeRetrieved && tries < _userSettings.DownloadMaxTries);

        return size;
    }
}
