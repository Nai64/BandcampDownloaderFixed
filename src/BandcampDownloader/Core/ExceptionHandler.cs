using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using BandcampDownloader.Properties;
using BandcampDownloader.Settings;
using NLog;

namespace BandcampDownloader.Core;

internal interface IExceptionHandler
{
    void RegisterUnhandledExceptionHandler();
}

public sealed class ExceptionHandler : IExceptionHandler
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly ISettingsService _settingsService;

    public ExceptionHandler(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public void RegisterUnhandledExceptionHandler()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        LogExceptionAndInnerExceptionsRecursively((Exception)e.ExceptionObject);
        Debugger.Break();

        var userSettings = _settingsService.GetUserSettings();
        string errorMessage;
        if (userSettings.ShowDetailedErrorDialog)
        {
            errorMessage = BuildDetailedErrorMessage((Exception)e.ExceptionObject);
        }
        else
        {
            errorMessage = string.Format(Resources.messageBoxUnhandledException, Constants.URL_ISSUES);
        }

        MessageBox.Show(errorMessage, "Bandcamp Downloader", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private string BuildDetailedErrorMessage(Exception exception)
    {
        var sb = new StringBuilder();
        sb.AppendLine("An unhandled error occurred. The application will close now.");
        sb.AppendLine();
        BuildExceptionDetails(exception, sb, 0);
        sb.AppendLine();
        sb.AppendLine($"Please open a new issue with the content of your log file on {Constants.URL_ISSUES}");
        return sb.ToString();
    }

    private void BuildExceptionDetails(Exception exception, StringBuilder sb, int level)
    {
        string indent = new string(' ', level * 2);
        sb.AppendLine($"{indent}Exception Type: {exception.GetType().Name}");
        sb.AppendLine($"{indent}Message: {exception.Message}");
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            sb.AppendLine($"{indent}Stack Trace:");
            foreach (var line in exception.StackTrace.Split('\n'))
            {
                sb.AppendLine($"{indent}  {line.Trim()}");
            }
        }

        if (exception.InnerException != null)
        {
            sb.AppendLine();
            sb.AppendLine($"{indent}Inner Exception:");
            BuildExceptionDetails(exception.InnerException, sb, level + 1);
        }
    }

    /// <summary>
    /// Writes the specified <see cref="Exception" /> and all its InnerExceptions to the log.
    /// </summary>
    private void LogExceptionAndInnerExceptionsRecursively(Exception exception)
    {
        _logger.Log(LogLevel.Fatal, $"{exception.GetType()} {exception.Message}");
        _logger.Log(LogLevel.Fatal, exception.StackTrace ?? "No stack trace available");

        if (exception.InnerException != null)
        {
            LogExceptionAndInnerExceptionsRecursively(exception.InnerException);
        }
    }
}
