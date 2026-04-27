using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BandcampDownloader.Bandcamp.Extraction;

namespace BandcampDownloader.UI.Dialogs;

internal sealed partial class WindowDiscographySelection
{
    public WindowDiscographySelection(IReadOnlyCollection<AlbumInfo> albumInfos)
    {
        InitializeComponent();
        ListViewAlbums.ItemsSource = albumInfos;
        UpdateSelectedCount();
    }

    public IReadOnlyCollection<AlbumInfo> SelectedAlbums =>
        ((IReadOnlyCollection<AlbumInfo>)ListViewAlbums.ItemsSource)
            .Where(a => a.IsSelected)
            .ToList();

    private void ButtonSelectAll_Click(object sender, RoutedEventArgs e)
    {
        foreach (AlbumInfo album in ListViewAlbums.ItemsSource)
        {
            album.IsSelected = true;
        }
        ListViewAlbums.Items.Refresh();
        UpdateSelectedCount();
    }

    private void ButtonDeselectAll_Click(object sender, RoutedEventArgs e)
    {
        foreach (AlbumInfo album in ListViewAlbums.ItemsSource)
        {
            album.IsSelected = false;
        }
        ListViewAlbums.Items.Refresh();
        UpdateSelectedCount();
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
        var selectedCount = ((IReadOnlyCollection<AlbumInfo>)ListViewAlbums.ItemsSource).Count(a => a.IsSelected);
        var totalCount = ((IReadOnlyCollection<AlbumInfo>)ListViewAlbums.ItemsSource).Count;

        // Find the TextBlock in the selection buttons panel
        var textBlock = ((StackPanel)Content).Children[1] as StackPanel;
        if (textBlock != null)
        {
            var countTextBlock = textBlock.Children[2] as TextBlock;
            if (countTextBlock != null)
            {
                countTextBlock.Text = $"Selected: {selectedCount}/{totalCount}";
            }
        }
    }

    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
