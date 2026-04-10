namespace BandcampDownloader.Bandcamp.Download;

internal sealed class TrackFile
{
    public long BytesReceived { get; set; }
    public bool Downloaded { get; set; }
    public long Size { get; }
    public long AdjustedSize { get; }
    public string Url { get; }

    public TrackFile(string url, long bytesReceived, long size, long adjustedSize = 0)
    {
        Url = url;
        BytesReceived = bytesReceived;
        Size = size;
        AdjustedSize = adjustedSize > 0 ? adjustedSize : size;
    }
}
