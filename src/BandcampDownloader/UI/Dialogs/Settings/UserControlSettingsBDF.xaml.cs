using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BandcampDownloader.Core.DependencyInjection;
using BandcampDownloader.Settings;

namespace BandcampDownloader.UI.Dialogs.Settings;

internal sealed partial class UserControlSettingsBDF : IUserControlSettings
{
    public UserControlSettingsBDF()
    {
        var userSettings = DependencyInjectionHelper.GetService<ISettingsService>().GetUserSettings();

        InitializeComponent();
        // Save data context for bindings
        DataContext = userSettings;
        
        // Force settings to persist immediately when changed
        CheckBoxContinueOnError.Checked += (s, e) => SaveSettings();
        CheckBoxContinueOnError.Unchecked += (s, e) => SaveSettings();
        CheckBoxWaitForFileAfterDownload.Checked += (s, e) => SaveSettings();
        CheckBoxWaitForFileAfterDownload.Unchecked += (s, e) => SaveSettings();
        CheckBoxIgnoreTracksLongerThan.Checked += (s, e) => SaveSettings();
        CheckBoxIgnoreTracksLongerThan.Unchecked += (s, e) => SaveSettings();
        CheckBoxAssignAlbumArtistToAllTracks.Checked += (s, e) => SaveSettings();
        CheckBoxAssignAlbumArtistToAllTracks.Unchecked += (s, e) => SaveSettings();
        CheckBoxSplitVariousArtistsTrackTitles.Checked += (s, e) => SaveSettings();
        CheckBoxSplitVariousArtistsTrackTitles.Unchecked += (s, e) => SaveSettings();
        CheckBoxShowTrackCountText.Checked += (s, e) => SaveSettings();
        CheckBoxShowTrackCountText.Unchecked += (s, e) => SaveSettings();
        SliderIgnoreTracksLongerThanMinutes.ValueChanged += (s, e) => SaveSettings();
    }

    /// <summary>
    /// Cancels the changes already applied.
    /// </summary>
    public void CancelChanges()
    {
        // Nothing to "unapply"
    }

    /// <summary>
    /// Loads settings from _userSettings.
    /// </summary>
    public void LoadSettings(IUserSettings userSettings)
    {
        // Reload DataContext in case settings have changed
        DataContext = userSettings;
        // No need to call UpdateTarget, it is done automatically
    }

    /// <summary>
    /// Saves settings to _userSettings.
    /// </summary>
    public void SaveSettings()
    {
        // Force immediate persistence to .ini file
        CheckBoxContinueOnError.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxWaitForFileAfterDownload.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxIgnoreTracksLongerThan.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        SliderIgnoreTracksLongerThanMinutes.GetBindingExpression(Slider.ValueProperty)?.UpdateSource();
        CheckBoxAssignAlbumArtistToAllTracks.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxSplitVariousArtistsTrackTitles.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxShowTrackCountText.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
    }

    private void SliderIgnoreTracksLongerThanMinutes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        // This method is kept for potential future use
    }
}
