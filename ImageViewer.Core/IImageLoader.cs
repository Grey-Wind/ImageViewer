using System.Windows.Media.Imaging;

namespace ImageViewer.Core
{
    public interface IImageLoader
    {
        BitmapSource LoadImage(string path);
        IEnumerable<string> SupportedFormats { get; }
    }
}