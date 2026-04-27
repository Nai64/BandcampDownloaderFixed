using System;
using System.Reflection;

namespace BandcampDownloader.Core;

internal static class Constants
{
    /// <summary>
    /// The version of BandcampDownloader.
    /// </summary>
    public static readonly Version APP_VERSION = Assembly.GetEntryAssembly()?.GetName().Version;

    /// <summary>
    /// The formatted version (X.Y.Z) of BandcampDownloader.
    /// </summary>
    public static readonly string APP_VERSION_FORMATTED = APP_VERSION.ToString(3);

    /// <summary>
    /// The URL redirecting to the help page on translating the app on GitHub.
    /// </summary>
    public const string URL_HELP_TRANSLATE = "https://github.com/Nai64/BandcampDownloaderFixed/blob/master/docs/help-translate.md";

    /// <summary>
    /// The URL redirecting to the issues page on GitHub.
    /// </summary>
    public const string URL_ISSUES = "https://github.com/Nai64/BandcampDownloaderFixed/issues";

    /// <summary>
    /// The URL redirecting to the latest release on GitHub.
    /// </summary>
    public const string URL_LATEST_RELEASE = "https://github.com/Nai64/BandcampDownloaderFixed/releases/latest";

    /// <summary>
    /// The URL redirecting to the releases page on GitHub.
    /// </summary>
    public const string URL_RELEASES = "https://github.com/Nai64/BandcampDownloaderFixed/releases";

    /// <summary>
    /// The website URL of BandcampDownloader.
    /// </summary>
    public const string URL_WEBSITE = "https://github.com/Nai64/BandcampDownloaderFixed";

    /// <summary>
    /// The URL of the original BandcampDownloader project.
    /// </summary>
    public const string URL_ORIGINAL_REPO = "https://github.com/otiel/bandcampdownloader";
}
