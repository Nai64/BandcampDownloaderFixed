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
        ComboBoxLanguage.ItemsSource = System.Enum.GetValues(typeof(LanguageEnum));
        ComboBoxLanguage.SelectedItem = LanguageEnum.en;
    }

    private void PopulateThemeComboBox()
    {
        ComboBoxTheme.ItemsSource = System.Enum.GetValues(typeof(Skin));
        ComboBoxTheme.SelectedItem = Skin.Light;
    }

    private void ButtonNext_Click(object sender, RoutedEventArgs e)
    {
        SelectedLanguage = (LanguageEnum)ComboBoxLanguage.SelectedItem;
        SelectedTheme = (Skin)ComboBoxTheme.SelectedItem;
        DialogResult = true;
        Close();
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
