using BandcampDownloader.Settings;

namespace BandcampDownloader.Core.Setup;

public enum SetupMode
{
    Simple,
    Moderate,
    Full,
    Custom
}

internal interface ISetupService
{
    void ApplySetupMode(SetupMode mode, IUserSettings userSettings);
}

internal sealed class SetupService : ISetupService
{
    public void ApplySetupMode(SetupMode mode, IUserSettings userSettings)
    {
        switch (mode)
        {
            case SetupMode.Simple:
                ApplySimpleMode(userSettings);
                break;
            case SetupMode.Moderate:
                ApplyModerateMode(userSettings);
                break;
            case SetupMode.Full:
                ApplyFullMode(userSettings);
                break;
            case SetupMode.Custom:
                // Don't apply any preset - leave as default
                break;
        }
    }

    private static void ApplySimpleMode(IUserSettings userSettings)
    {
        // Basic functionality only
        userSettings.ModifyTags = false;
        userSettings.SaveCoverArtInFolder = false;
        userSettings.SaveCoverArtInTags = false;
        userSettings.CreatePlaylist = false;
        userSettings.SplitVariousArtistsTrackTitles = false;
        userSettings.DownloadArtistDiscography = false;
        userSettings.EnableDiscographySelectionDialog = false;
        userSettings.RetrieveFilesSize = false;
        userSettings.ContinueOnError = false;
        userSettings.WaitForFileAfterDownload = false;
        userSettings.EnableToastNotifications = false;
        userSettings.ShowVerboseLog = false;
        userSettings.ShowDetailedErrorDialog = false;
        userSettings.RememberLastUrl = false;
    }

    private static void ApplyModerateMode(IUserSettings userSettings)
    {
        // Recommended settings for most users
        userSettings.ModifyTags = true;
        userSettings.SaveCoverArtInFolder = false;
        userSettings.SaveCoverArtInTags = true;
        userSettings.CreatePlaylist = false;
        userSettings.SplitVariousArtistsTrackTitles = true;
        userSettings.DownloadArtistDiscography = true;
        userSettings.EnableDiscographySelectionDialog = true;
        userSettings.RetrieveFilesSize = true;
        userSettings.ContinueOnError = true;
        userSettings.WaitForFileAfterDownload = true;
        userSettings.EnableToastNotifications = false;
        userSettings.ShowVerboseLog = false;
        userSettings.ShowDetailedErrorDialog = false;
        userSettings.RememberLastUrl = false;
    }

    private static void ApplyFullMode(IUserSettings userSettings)
    {
        // All features enabled
        userSettings.ModifyTags = true;
        userSettings.SaveCoverArtInFolder = true;
        userSettings.SaveCoverArtInTags = true;
        userSettings.CreatePlaylist = true;
        userSettings.SplitVariousArtistsTrackTitles = true;
        userSettings.DownloadArtistDiscography = true;
        userSettings.EnableDiscographySelectionDialog = true;
        userSettings.RetrieveFilesSize = true;
        userSettings.ContinueOnError = true;
        userSettings.WaitForFileAfterDownload = true;
        userSettings.EnableToastNotifications = true;
        userSettings.ToastOnDownloadComplete = true;
        userSettings.ToastOnDownloadError = true;
        userSettings.ToastOnAlbumComplete = true;
        userSettings.ToastOnTrackSkipped = true;
        userSettings.ShowVerboseLog = false;
        userSettings.ShowDetailedErrorDialog = true;
        userSettings.RememberLastUrl = true;
    }
}
