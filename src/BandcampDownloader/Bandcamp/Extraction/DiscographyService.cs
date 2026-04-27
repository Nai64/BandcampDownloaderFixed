using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using NLog;

namespace BandcampDownloader.Bandcamp.Extraction;

internal interface IDiscographyService
{
    /// <summary>
    /// Returns all the albums URLs existing on the specified "/music" Bandcamp page.
    /// </summary>
    /// <param name="musicPageHtmlContent">The HTML source code of the "/music" Bandcamp page of an artist.</param>
    IReadOnlyCollection<string> GetReferredAlbumsRelativeUrls(string musicPageHtmlContent);

    /// <summary>
    /// Returns all the album information existing on the specified "/music" Bandcamp page.
    /// </summary>
    /// <param name="musicPageHtmlContent">The HTML source code of the "/music" Bandcamp page of an artist.</param>
    IReadOnlyCollection<AlbumInfo> GetReferredAlbumsInfo(string musicPageHtmlContent);
}

internal sealed class DiscographyService : IDiscographyService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public IReadOnlyCollection<string> GetReferredAlbumsRelativeUrls(string musicPageHtmlContent)
    {
        if (IsSingleAlbumArtist(musicPageHtmlContent))
        {
            _logger.Info("Found single album artist when looking for its discography");

            var albumUrl = GetAlbumUrlFromSingleAlbumArtist(musicPageHtmlContent);

            _logger.Info($"Found album for a single album artist with the following relative URL: {albumUrl}");

            return [albumUrl];
        }

        var regex = new Regex("(?<url>/(album|track)/.+?)(\"|&quot;)");
        if (!regex.IsMatch(musicPageHtmlContent))
        {
            throw new NoAlbumFoundException();
        }

        var albumsUrl = new List<string>();
        foreach (Match m in regex.Matches(musicPageHtmlContent))
        {
            albumsUrl.Add(m.Groups["url"].Value);
        }

        // Remove duplicates
        var distinctAlbumsUrl = albumsUrl.Distinct().ToList();

        return distinctAlbumsUrl;
    }

    public IReadOnlyCollection<AlbumInfo> GetReferredAlbumsInfo(string musicPageHtmlContent)
    {
        // Try to parse JSON data from data-client-items attribute
        var jsonDataRegex = new Regex("data-client-items=\"(?<data>[^\"]+)\"");
        var jsonDataMatch = jsonDataRegex.Match(musicPageHtmlContent);

        if (jsonDataMatch.Success)
        {
            try
            {
                var jsonData = jsonDataMatch.Groups["data"].Value;
                // Unescape HTML entities
                jsonData = jsonData.Replace("&quot;", "\"");
                var albumInfos = JsonSerializer.Deserialize<List<JsonAlbumData>>(jsonData);

                if (albumInfos != null && albumInfos.Count > 0)
                {
                    return albumInfos.Select(data => new AlbumInfo
                    {
                        Artist = data.Artist ?? "Unknown",
                        Title = data.Title ?? "Unknown",
                        RelativeUrl = data.PageUrl ?? "",
                        Type = data.Type ?? "album"
                    }).ToList();
                }
            }
            catch (JsonException ex)
            {
                _logger.Warn(ex, "Failed to parse JSON data from music page, falling back to regex method");
            }
        }

        // Fallback to regex method if JSON parsing fails
        _logger.Info("Using regex fallback method for album info extraction");
        var urls = GetReferredAlbumsRelativeUrls(musicPageHtmlContent);
        return urls.Select(url => new AlbumInfo
        {
            Artist = "Unknown",
            Title = ExtractTitleFromUrl(url),
            RelativeUrl = url,
            Type = url.Contains("/track/") ? "track" : "album"
        }).ToList();
    }

    private static string ExtractTitleFromUrl(string url)
    {
        // Extract title from URL like "/album/album-name" or "/track/track-name"
        var parts = url.Split('/');
        if (parts.Length >= 3)
        {
            return parts[2].Replace("-", " ");
        }
        return "Unknown";
    }

    // JSON data structure for deserialization
    private sealed class JsonAlbumData
    {
        public string Artist { get; set; } = "";
        public string Title { get; set; } = "";
        public string PageUrl { get; set; } = "";
        public string Type { get; set; } = "";
    }

    private static bool IsSingleAlbumArtist(string musicPageHtmlContent)
    {
        // When the artist has a single album, his music page often redirects to his album page
        // We look for 'div id="discography"' to determine if we're on an album page
        return musicPageHtmlContent.Contains("div id=\"discography\"");
    }

    private string GetAlbumUrlFromSingleAlbumArtist(string musicPageHtmlContent)
    {
        var regex = new Regex("href=\"(?<url>/album/.+?)\"");
        var matches = regex.Matches(musicPageHtmlContent);
        var albumUrls = matches.Select(m => m.Groups["url"].Value);
        var distinctAlbumUrls = albumUrls.Distinct().ToList();

        switch (distinctAlbumUrls.Count)
        {
            case 0:
                _logger.Error("No album URL found whereas we expected to find exactly one");
                throw new NoAlbumFoundException();
            case 1:
                return distinctAlbumUrls.Single();
            default:
                _logger.Error("Found multiple album URLs whereas we expected to find exactly one");
                throw new NoAlbumFoundException();
        }
    }
}
