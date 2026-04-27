namespace BandcampDownloader.Bandcamp.Extraction;

/// <summary>
/// Represents information about a Bandcamp album or track.
/// </summary>
internal sealed class AlbumInfo
{
    /// <summary>
    /// The artist name.
    /// </summary>
    public string Artist { get; set; } = string.Empty;

    /// <summary>
    /// The album or track title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The relative URL (e.g., "/album/album-name").
    /// </summary>
    public string RelativeUrl { get; set; } = string.Empty;

    /// <summary>
    /// The type (album or track).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Whether this item is selected for download.
    /// </summary>
    public bool IsSelected { get; set; } = true;

    /// <summary>
    /// The full URL.
    /// </summary>
    public string GetFullUrl(string artistBaseUrl)
    {
        return artistBaseUrl + RelativeUrl;
    }
}
