using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BandcampDownloader.Core.DependencyInjection;
using BandcampDownloader.Settings;

namespace BandcampDownloader.UI.Dialogs.Settings;

internal sealed partial class UserControlSettingsBDF : IUserControlSettings
{
    private readonly IUserSettings _userSettings;
    private bool _isInitialLoad = true;

    public UserControlSettingsBDF()
    {
        _userSettings = DependencyInjectionHelper.GetService<ISettingsService>().GetUserSettings();

        InitializeComponent();
        // Save data context for bindings
        DataContext = _userSettings;
        
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
        CheckBoxForceBitrate.Checked += CheckBoxForceBitrate_Checked;
        CheckBoxForceBitrate.Unchecked += (s, e) => SaveSettings();
        ComboBoxBitrate.SelectionChanged += (s, e) => SaveSettings();
        CheckBoxShowOriginalDownloadSize.Checked += (s, e) => SaveSettings();
        CheckBoxShowOriginalDownloadSize.Unchecked += (s, e) => SaveSettings();
        CheckBoxEnableToastNotifications.Checked += (s, e) => SaveSettings();
        CheckBoxEnableToastNotifications.Unchecked += (s, e) => SaveSettings();
        CheckBoxToastOnDownloadComplete.Checked += (s, e) => SaveSettings();
        CheckBoxToastOnDownloadComplete.Unchecked += (s, e) => SaveSettings();
        CheckBoxToastOnDownloadError.Checked += (s, e) => SaveSettings();
        CheckBoxToastOnDownloadError.Unchecked += (s, e) => SaveSettings();
        CheckBoxToastOnAlbumComplete.Checked += (s, e) => SaveSettings();
        CheckBoxToastOnAlbumComplete.Unchecked += (s, e) => SaveSettings();
        CheckBoxToastOnTrackSkipped.Checked += (s, e) => SaveSettings();
        CheckBoxToastOnTrackSkipped.Unchecked += (s, e) => SaveSettings();
        CheckBoxEnableDiscographySelectionDialog.Checked += (s, e) => SaveSettings();
        CheckBoxEnableDiscographySelectionDialog.Unchecked += (s, e) => SaveSettings();
        CheckBoxRememberLastUrl.Checked += (s, e) => SaveSettings();
        CheckBoxRememberLastUrl.Unchecked += (s, e) => SaveSettings();

        // Mark initial load as complete after UI is ready
        Dispatcher.BeginInvoke(new Action(() => _isInitialLoad = false), System.Windows.Threading.DispatcherPriority.Loaded);
    }

    private void CheckBoxForceBitrate_Checked(object sender, RoutedEventArgs e)
    {
        // Skip warning during initial load
        if (_isInitialLoad)
        {
            SaveSettings();
            return;
        }
        
        // Check if warning should be shown
        if (!_userSettings.SuppressForceBitrateWarning)
        {
            // Create custom dialog with checkbox
            var dialog = new Window
            {
                Title = "Experimental Feature Warning",
                Width = 400,
                Height = 220,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            var mainPanel = new StackPanel { Margin = new Thickness(15) };
            
            var warningText = new TextBlock
            {
                Text = "This feature is experimental. Bitrate conversion may not work correctly on all systems and could produce unexpected results.\n\nAre you sure you want to enable this feature?",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 15)
            };
            
            var dontShowAgainCheckBox = new CheckBox
            {
                Content = "Don't show this warning again",
                Margin = new Thickness(0, 0, 0, 15)
            };
            
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            
            var yesButton = new Button
            {
                Content = "_Yes",
                Width = 75,
                Margin = new Thickness(0, 0, 10, 0),
                IsDefault = true
            };
            yesButton.Click += (s, args) => {
                if (dontShowAgainCheckBox.IsChecked == true)
                {
                    _userSettings.SuppressForceBitrateWarning = true;
                    SaveSettings();
                }
                dialog.DialogResult = true;
                dialog.Close();
            };
            
            var noButton = new Button
            {
                Content = "_No",
                Width = 75,
                IsCancel = true
            };
            noButton.Click += (s, args) => {
                CheckBoxForceBitrate.IsChecked = false;
                dialog.DialogResult = false;
                dialog.Close();
            };
            
            buttonPanel.Children.Add(yesButton);
            buttonPanel.Children.Add(noButton);
            
            mainPanel.Children.Add(warningText);
            mainPanel.Children.Add(dontShowAgainCheckBox);
            mainPanel.Children.Add(buttonPanel);
            
            dialog.Content = mainPanel;
            
            // Show dialog
            dialog.ShowDialog();
        }
        else
        {
            SaveSettings();
        }
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
        CheckBoxForceBitrate.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        ComboBoxBitrate.GetBindingExpression(Selector.SelectedValueProperty)?.UpdateSource();
        CheckBoxShowOriginalDownloadSize.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxEnableToastNotifications.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxToastOnDownloadComplete.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxToastOnDownloadError.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxToastOnAlbumComplete.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxToastOnTrackSkipped.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxEnableDiscographySelectionDialog.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
        CheckBoxRememberLastUrl.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateSource();
    }

    private void ComboBoxBitrate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ComboBoxBitrate.SelectedValue is string selectedBitrate && int.TryParse(selectedBitrate, out int bitrate) && bitrate > 128)
        {
            var result = MessageBox.Show(
                "Note that upsampling won't add quality that isn't there. The source audio is 128 kbps.\n\nAre you sure you want to continue?",
                "Upsampling Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                // Revert to 128 kbps
                ComboBoxBitrate.SelectedValue = "128";
            }
        }
    }
}
