using System;
using System.Windows;
using BandcampDownloader.Settings;

namespace BandcampDownloader.Core.Themes;

internal interface IThemeService
{
    /// <summary>
    /// Applies the specified <see cref="Skin" /> to the application resources.
    /// </summary>
    void ApplySkin(Skin skin);
}

internal sealed class ThemeService : IThemeService
{
    public void ApplySkin(Skin skin)
    {
        Application.Current.Resources.MergedDictionaries.Clear();

        switch (skin)
        {
            case Skin.Dark:
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Core\\Themes\\DarkTheme.xaml", UriKind.Relative) });
                break;
            case Skin.Light:
                // Do nothing
                break;
            case Skin.Blue:
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Core\\Themes\\BlueTheme.xaml", UriKind.Relative) });
                break;
            case Skin.Green:
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Core\\Themes\\GreenTheme.xaml", UriKind.Relative) });
                break;
            case Skin.Purple:
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Core\\Themes\\PurpleTheme.xaml", UriKind.Relative) });
                break;
            case Skin.Orange:
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Core\\Themes\\OrangeTheme.xaml", UriKind.Relative) });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skin), skin, null);
        }
    }
}
