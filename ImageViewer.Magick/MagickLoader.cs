using ImageMagick;
using ImageViewer.Core;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageViewer.Magick
{
    public class MagickLoader : IImageLoader
    {
        public IEnumerable<string> SupportedFormats =>
        [
        "jpg", "jpeg", "tif", "tiff", "png", "gif", "bmp", "dib",
        "jpe", "jfif", "webp", "rle", "dcm", "dc3", "dic", "eps",
        "iff", "tdi", "jpf", "jpx", "jp2", "j2c", "j2k", "jpc",
        "pcx", "raw", "pxr", "pbm", "pgm", "ppm", "pnm", "pfm",
        "pam", "sct", "tga", "vda", "icb", "vst"
    ];

        public BitmapSource LoadImage(string path)
        {
            using var image = new MagickImage(path);
            return CreateBitmapSource(image);
        }

        private static BitmapImage CreateBitmapSource(MagickImage image)
        {
            image.Format = MagickFormat.Bmp;
            var bitmap = new BitmapImage();
            using var stream = new MemoryStream(image.ToByteArray());
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
    }
}
