using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BandcampDownloader.Bandcamp.Extraction;
using BandcampDownloader.Net;
using NLog;

namespace BandcampDownloader.Bandcamp.Download;

internal interface IAlbumUrlRetriever
{
    Task<IReadOnlyCollection<string>> RetrieveAlbumsUrlsAsync(string inputUrls, bool downloadArtistDiscography, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AlbumInfo>> RetrieveAlbumsInfoAsync(string inputUrls, bool downloadArtistDiscography, CancellationToken cancellationToken);
    event DownloadProgressChangedEventHandler DownloadProgressChanged;
}

internal sealed class AlbumUrlRetriever : IAlbumUrlRetriever
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IDiscographyService _discographyService;
    private readonly IHttpService _httpService;
    public event DownloadProgressChangedEventHandler DownloadProgressChanged;

    public AlbumUrlRetriever(IDiscographyService discographyService, IHttpService httpService)
    {
        _discographyService = discographyService;
        _httpService = httpService;
    }

    public async Task<IReadOnlyCollection<string>> RetrieveAlbumsUrlsAsync(string inputUrls, bool downloadArtistDiscography, CancellationToken cancellationToken)
    {
        var splitUrls = inputUrls.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).ToList();
        var sanitizedUrls = splitUrls.Distinct().Select(o => o.Trim()).ToList();

        if (!downloadArtistDiscography)
        {
            return sanitizedUrls;
        }

        var albumsUrls = await GetArtistDiscographyAlbumsUrlsAsync(sanitizedUrls, cancellationToken).ConfigureAwait(false);
        return albumsUrls;
    }

    public async Task<IReadOnlyCollection<AlbumInfo>> RetrieveAlbumsInfoAsync(string inputUrls, bool downloadArtistDiscography, CancellationToken cancellationToken)
    {
        var splitUrls = inputUrls.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).ToList();
        var sanitizedUrls = splitUrls.Distinct().Select(o => o.Trim()).ToList();

        if (!downloadArtistDiscography)
        {
            // Convert URLs to AlbumInfo objects
            return sanitizedUrls.Select(url => new AlbumInfo
            {
                Title = ExtractTitleFromUrl(url),
                RelativeUrl = new Uri(url).PathAndQuery,
                Type = url.Contains("/track/") ? "track" : "album",
                IsSelected = true
            }).ToList();
        }

        var albumInfos = await GetArtistDiscographyAlbumsInfoAsync(sanitizedUrls, cancellationToken).ConfigureAwait(false);
        return albumInfos;
    }

    private static string ExtractTitleFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            if (segments.Length >= 3)
            {
                return segments[2].Trim('/').Replace("-", " ");
            }
        }
        catch
        {
            // Ignore URL parsing errors
        }
        return "Unknown";
    }

    /// <summary>
    /// Returns the artists discography from any URL (artist, album, track).
    /// </summary>
    private async Task<IReadOnlyCollection<string>> GetArtistDiscographyAlbumsUrlsAsync(IReadOnlyCollection<string> urls, CancellationToken cancellationToken)
    {
        var albumsUrls = new List<string>();

        foreach (var url in urls)
        {
            cancellationToken.ThrowIfCancellationRequested();

            DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Retrieving artist discography from {url}", DownloadProgressChangedLevel.Info));

            // Get artist "music" bandcamp page (http://artist.bandcamp.com/music)
            var regex = new Regex("https?://[^/]*");
            var artistPage = regex.Match(url).ToString();
            var artistMusicPage = artistPage + "/music";

            // Retrieve artist "music" page HTML source code
            string htmlContent;

            try
            {
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Downloading album info from url: {url}", DownloadProgressChangedLevel.VerboseInfo));
                var httpClient = _httpService.CreateHttpClient();
                htmlContent = await httpClient.GetStringAsync(artistMusicPage, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.Error(ex);
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Could not retrieve data for {artistMusicPage}", DownloadProgressChangedLevel.Error));
                continue;
            }

            try
            {
                var relativeAlbumsUrl = _discographyService.GetReferredAlbumsRelativeUrls(htmlContent);
                var albumsUrl = relativeAlbumsUrl.Select(o => $"{artistPage}{o}");
                albumsUrls.AddRange(albumsUrl);
            }
            catch (NoAlbumFoundException ex)
            {
                _logger.Error(ex);
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"No referred album could be found on {artistMusicPage}. Try to uncheck the \"Download artist discography\" option", DownloadProgressChangedLevel.Error));
            }
        }

        return albumsUrls.Distinct().ToList();
    }

    /// <summary>
    /// Returns the artists discography with album information from any URL (artist, album, track).
    /// </summary>
    private async Task<IReadOnlyCollection<AlbumInfo>> GetArtistDiscographyAlbumsInfoAsync(IReadOnlyCollection<string> urls, CancellationToken cancellationToken)
    {
        var albumInfos = new List<AlbumInfo>();

        foreach (var url in urls)
        {
            cancellationToken.ThrowIfCancellationRequested();

            DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Retrieving artist discography from {url}", DownloadProgressChangedLevel.Info));

            // Get artist "music" bandcamp page (http://artist.bandcamp.com/music)
            var regex = new Regex("https?://[^/]*");
            var artistPage = regex.Match(url).ToString();
            var artistMusicPage = artistPage + "/music";

            // Retrieve artist "music" page HTML source code
            string htmlContent;

            try
            {
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Downloading album info from url: {url}", DownloadProgressChangedLevel.VerboseInfo));
                var httpClient = _httpService.CreateHttpClient();
                htmlContent = await httpClient.GetStringAsync(artistMusicPage, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.Error(ex);
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"Could not retrieve data for {artistMusicPage}", DownloadProgressChangedLevel.Error));
                continue;
            }

            try
            {
                var albumsInfo = _discographyService.GetReferredAlbumsInfo(htmlContent);
                // Set full URLs for each album
                foreach (var albumInfo in albumsInfo)
                {
                    albumInfo.RelativeUrl = albumInfo.RelativeUrl; // Keep relative URL
                }
                albumInfos.AddRange(albumsInfo);
            }
            catch (NoAlbumFoundException ex)
            {
                _logger.Error(ex);
                DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedArgs($"No referred album could be found on {artistMusicPage}. Try to uncheck the \"Download artist discography\" option", DownloadProgressChangedLevel.Error));
            }
        }

        return albumInfos.Distinct().ToList();
    }
}
