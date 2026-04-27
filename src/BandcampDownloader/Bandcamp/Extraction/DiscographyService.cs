using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <param name="artistBaseUrl">Optional artist name to use for all albums.</param>
    IReadOnlyCollection<AlbumInfo> GetReferredAlbumsInfo(string musicPageHtmlContent, string artistBaseUrl = "");
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

    public IReadOnlyCollection<AlbumInfo> GetReferredAlbumsInfo(string musicPageHtmlContent, string artistBaseUrl = "")
    {
        // Use regex method to get all releases (JSON only contains first few)
        _logger.Info("Using regex method for album info extraction to get all releases");
        var urls = GetReferredAlbumsRelativeUrls(musicPageHtmlContent);
        
        var regexResult = urls.Select(url => new AlbumInfo
        {
            Title = ExtractTitleFromUrl(url),
            RelativeUrl = url,
            Type = url.Contains("/track/") ? "track" : "album"
        }).ToList();

        // Log first album for debugging
        if (regexResult.Count > 0)
        {
            _logger.Info($"First album (regex): Title='{regexResult[0].Title}', Type='{regexResult[0].Type}', URL='{regexResult[0].RelativeUrl}'");
            _logger.Info($"Total albums found via regex: {regexResult.Count}");
        }

        return regexResult;
    }

    private static string ExtractTitleFromUrl(string url)
    {
        // Extract title from URL like "/album/album-name" or "/track/track-name"
        var parts = url.Split('/');
        if (parts.Length >= 3)
        {
            var title = parts[2].Replace("-", " ");
            return ToTitleCase(title);
        }
        return "Unknown";
    }

    private static string ToTitleCase(string str)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }

    // JSON data structure for deserialization
    private sealed class JsonAlbumData
    {
        public int ArtId { get; set; }
        public int BandId { get; set; }
        public int Id { get; set; }
        public string PageUrl { get; set; } = "";
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        // Note: Artist name is not in the JSON, need to get it from page or use band name
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
