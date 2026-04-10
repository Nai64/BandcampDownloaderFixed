using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using NLog;

namespace BandcampDownloader.UI;

internal static class ToastNotifier
{
    private static NotifyIcon _notifyIcon;
    private static readonly object _lock = new object();

    private static NotifyIcon GetNotifyIcon()
    {
        lock (_lock)
        {
            if (_notifyIcon == null)
            {
                // Try to load the app icon from the executable
                Icon appIcon = null;
                var exePath = Path.Combine(AppContext.BaseDirectory, "BandcampDownloader.exe");
                if (File.Exists(exePath))
                {
                    try 
                    {
                        appIcon = Icon.ExtractAssociatedIcon(exePath);
                    }
                    catch { }
                }

                _notifyIcon = new NotifyIcon
                {
                    Icon = appIcon ?? SystemIcons.Application,
                    Visible = true
                };
            }
            return _notifyIcon;
        }
    }

    public static void ShowDownloadComplete(int albumCount, int trackCount)
    {
        try
        {
            var notifyIcon = GetNotifyIcon();
            
            // Show on UI thread
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                notifyIcon.BalloonTipTitle = "BandcampDownloader - Download Complete";
                notifyIcon.BalloonTipText = $"Downloaded {trackCount} track{(trackCount != 1 ? "s" : "")} from {albumCount} album{(albumCount != 1 ? "s" : "")}";
                notifyIcon.ShowBalloonTip(3000);
            });
        }
        catch (Exception ex)
        {
            LogManager.GetCurrentClassLogger().Debug(ex, "Failed to show notification");
        }
    }

    public static void ShowAlbumComplete(string albumTitle, int trackCount)
    {
        try
        {
            var notifyIcon = GetNotifyIcon();
            
            // Show on UI thread
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                notifyIcon.BalloonTipTitle = $"Album Downloaded: {albumTitle}";
                notifyIcon.BalloonTipText = $"Successfully downloaded {trackCount} track{(trackCount != 1 ? "s" : "")}";
                notifyIcon.ShowBalloonTip(3000);
            });
        }
        catch (Exception ex)
        {
            LogManager.GetCurrentClassLogger().Debug(ex, "Failed to show notification");
        }
    }

    public static void ShowDownloadError(string errorMessage)
    {
        try
        {
            var notifyIcon = GetNotifyIcon();
            
            // Show on UI thread
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                notifyIcon.Icon = SystemIcons.Warning;
                notifyIcon.BalloonTipTitle = "BandcampDownloader - Error";
                notifyIcon.BalloonTipText = errorMessage;
                notifyIcon.ShowBalloonTip(5000);
                
                // Reset icon back to info for next notification
                notifyIcon.Icon = SystemIcons.Information;
            });
        }
        catch (Exception ex)
        {
            LogManager.GetCurrentClassLogger().Debug(ex, "Failed to show notification");
        }
    }

    public static void ShowTrackSkipped(string trackTitle, string reason)
    {
        try
        {
            var notifyIcon = GetNotifyIcon();
            
            // Show on UI thread
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                notifyIcon.Icon = SystemIcons.Warning;
                notifyIcon.BalloonTipTitle = "Track Skipped";
                notifyIcon.BalloonTipText = $"\"{trackTitle}\" - {reason}";
                notifyIcon.ShowBalloonTip(5000);
                
                // Reset icon back to info for next notification
                notifyIcon.Icon = SystemIcons.Information;
            });
        }
        catch (Exception ex)
        {
            LogManager.GetCurrentClassLogger().Debug(ex, "Failed to show notification");
        }
    }
}

