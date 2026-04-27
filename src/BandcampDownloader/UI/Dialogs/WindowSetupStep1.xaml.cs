using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using BandcampDownloader.Settings;
using LanguageEnum = BandcampDownloader.Settings.Language;

namespace BandcampDownloader.UI.Dialogs;

internal sealed partial class WindowSetupStep1
{
    public LanguageEnum SelectedLanguage { get; private set; }
    public Skin SelectedTheme { get; private set; }

    public WindowSetupStep1()
    {
        InitializeComponent();
        SelectedLanguage = LanguageEnum.en;
        SelectedTheme = Skin.Light;
        PopulateLanguageComboBox();
        PopulateThemeComboBox();
    }

    private void PopulateLanguageComboBox()
    {
        ComboBoxLanguage.ItemsSource = GetEnumDescriptions<LanguageEnum>();
        ComboBoxLanguage.SelectedItem = GetEnumDescription(LanguageEnum.en);
    }

    private void PopulateThemeComboBox()
    {
        ComboBoxTheme.ItemsSource = GetEnumDescriptions<Skin>();
        ComboBoxTheme.SelectedItem = GetEnumDescription(Skin.Light);
    }

    private static string GetEnumDescription<T>(T value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }

    private static List<string> GetEnumDescriptions<T>()
    {
        return System.Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(GetEnumDescription)
            .ToList();
    }

    private void ButtonNext_Click(object sender, RoutedEventArgs e)
    {
        var languageDescription = (string)ComboBoxLanguage.SelectedItem;
        SelectedLanguage = System.Enum.GetValues<LanguageEnum>()
            .First(l => GetEnumDescription(l) == languageDescription);

        var themeDescription = (string)ComboBoxTheme.SelectedItem;
        SelectedTheme = System.Enum.GetValues<Skin>()
            .First(t => GetEnumDescription(t) == themeDescription);

        DialogResult = true;
        Close();
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
