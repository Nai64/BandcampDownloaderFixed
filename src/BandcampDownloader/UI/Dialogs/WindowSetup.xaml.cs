using System.Windows;
using BandcampDownloader.Core.Setup;

namespace BandcampDownloader.UI.Dialogs;

internal sealed partial class WindowSetup
{
    public SetupMode SelectedMode { get; private set; } = SetupMode.Moderate;

    public WindowSetup()
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

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
