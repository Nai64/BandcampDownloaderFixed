using System.Runtime.InteropServices;
using System.Windows;
using BandcampDownloader.Core;
using BandcampDownloader.Core.DependencyInjection;
using BandcampDownloader.Core.Localization;
using BandcampDownloader.Core.Logging;
using BandcampDownloader.Core.Setup;
using BandcampDownloader.Core.Themes;
using BandcampDownloader.Settings;
using BandcampDownloader.UI.Dialogs;
using NLog;

namespace BandcampDownloader;

internal sealed partial class App
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private static bool _coreServicesInitialized = false;

    protected override void OnStartup(StartupEventArgs e)
    {
        var container = DependencyInjectionHelper.InitializeContainer();

        // 1. Initialize the logger first in order to be able to log any early errors
        var loggingService = container.GetService<ILoggingService>();
        loggingService.InitializeLogger();

        // 2. Register the handler for unhandled exception in order to catch and log early unhandled exceptions
        var exceptionHandler = container.GetService<IExceptionHandler>();
        exceptionHandler.RegisterUnhandledExceptionHandler();

        // 3. Log the application properties
        LogAppProperties();

        // 4. Initialize settings first
        var settingsService = container.GetService<ISettingsService>();
        settingsService.InitializeSettings();

        // 5. Check if first-time user and show setup dialog
        ShowSetupDialogIfNeeded(container);

        // 6. Initialize less critical services (only if not already initialized by setup dialog)
        if (!_coreServicesInitialized)
        {
            InitializeCoreServices(container);
        }

        // 7. Log the user settings
        LogUserSettings(container);

        // 8. Open the main window
        var windowMain = container.GetService<WindowMain>();
        windowMain.Show();
    }

    private static void InitializeCoreServices(IContainer container)
    {
        var settingsService = container.GetService<ISettingsService>();
        var userSettings = settingsService.GetUserSettings();

        var languageService = container.GetService<ILanguageService>();
        languageService.ApplyLanguage(userSettings.Language);

        var themeService = container.GetService<IThemeService>();
        themeService.ApplySkin(userSettings.Theme);

        _coreServicesInitialized = true;
    }

    private static void LogAppProperties()
    {
        _logger.Info("┳┓     ┓          ┳┓       ┓     ┓");
        _logger.Info("┣┫┏┓┏┓┏┫┏┏┓┏┳┓┏┓  ┃┃┏┓┓┏┏┏┓┃┏┓┏┓┏┫┏┓┏┓");
        _logger.Info("┻┛┗┻┛┗┗┻┗┗┻┛┗┗┣┛  ┻┛┗┛┗┻┛┛┗┗┗┛┗┻┗┻┗ ┛");
        _logger.Info("              ┛");
        _logger.Info($"BandcampDownloader version: {Constants.APP_VERSION_FORMATTED}");
        _logger.Info($"Framework description: {RuntimeInformation.FrameworkDescription}");
        _logger.Info($"OS architecture: {RuntimeInformation.OSArchitecture}");
        _logger.Info($"OS description: {RuntimeInformation.OSDescription}");
        _logger.Info($"Process architecture: {RuntimeInformation.ProcessArchitecture}");
        _logger.Info($"Runtime identifier: {RuntimeInformation.RuntimeIdentifier}");
    }

    private static void LogUserSettings(IContainer container)
    {
        var settingsService = container.GetService<ISettingsService>();
        var userSettings = settingsService.GetUserSettings();
        var userSettingsJson = settingsService.GetUserSettingsInJson();

        _logger.Info($"Settings: {userSettingsJson}");
    }

    private static void ShowSetupDialogIfNeeded(IContainer container)
    {
        var settingsService = container.GetService<ISettingsService>();
        var userSettings = settingsService.GetUserSettings();

        if (!userSettings.HasCompletedSetup)
        {
            var setupService = container.GetService<ISetupService>();
            var themeService = container.GetService<IThemeService>();

            bool completed = false;
            while (!completed)
            {
                // Step 1: Language and Theme selection
                var windowSetupStep1 = new WindowSetupStep1(themeService)
                {
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                var result1 = windowSetupStep1.ShowDialog();
                if (result1 != true)
                {
                    // User cancelled, exit application
                    Application.Current.Shutdown();
                    return;
                }

                // Step 2: Feature mode selection
                var windowSetupStep2 = new WindowSetupStep2
                {
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                var result2 = windowSetupStep2.ShowDialog();
                if (result2 == true)
                {
                    // User clicked Continue, setup is complete
                    completed = true;

                    // Apply settings from both dialogs
                    setupService.ApplySetupMode(windowSetupStep2.SelectedMode, userSettings);
                    userSettings.Language = windowSetupStep1.SelectedLanguage;
                    userSettings.Theme = windowSetupStep1.SelectedTheme;
                    userSettings.HasCompletedSetup = true;

                    // Force save settings by resetting the settings service
                    settingsService.ResetSettings();
                    var finalSettings = settingsService.InitializeSettings();
                    finalSettings.HasCompletedSetup = true;
                    finalSettings.Theme = windowSetupStep1.SelectedTheme;
                    finalSettings.Language = windowSetupStep1.SelectedLanguage;
                    finalSettings.DownloadsPath = userSettings.DownloadsPath;
                    setupService.ApplySetupMode(windowSetupStep2.SelectedMode, finalSettings);
                    finalSettings.HasCompletedSetup = true;

                    // Re-initialize services with the new settings
                    InitializeCoreServices(container);
                }
                // If result2 is false, user clicked Previous Step, so loop back to step 1
            }
        }
    }
}
