using System.Windows;
using BandcampDownloader.Core.Setup;

namespace BandcampDownloader.UI.Dialogs;

internal sealed partial class WindowSetupStep2
{
    public SetupMode SelectedMode { get; private set; } = SetupMode.Moderate;

    public WindowSetupStep2()
    {
        InitializeComponent();
    }

    private void ButtonContinue_Click(object sender, RoutedEventArgs e)
    {
        if (RadioButtonSimple.IsChecked == true)
            SelectedMode = SetupMode.Simple;
        else if (RadioButtonModerate.IsChecked == true)
            SelectedMode = SetupMode.Moderate;
        else if (RadioButtonFull.IsChecked == true)
            SelectedMode = SetupMode.Full;
        else if (RadioButtonCustom.IsChecked == true)
            SelectedMode = SetupMode.Custom;

        DialogResult = true;
        Close();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
