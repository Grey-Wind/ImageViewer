using ImageViewer.Core;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using ImageMagick;

namespace ImageViewer.Magick.HDRI
{
    public class HdriMagickLoader : IImageLoader
    {
        public IEnumerable<string> SupportedFormats =>
        [
        "heic", "psd", "pdd", "psdt", "psb",  // HDRI专用格式
        "jpg", "jpeg", "tif", "tiff", "png", "gif", "bmp", "dib",
        "jpe", "jfif", "webp", "rle", "dcm", "dc3", "dic", "eps",
        "iff", "tdi", "jpf", "jpx", "jp2", "j2c", "j2k", "jpc",
        "pcx", "raw", "pxr", "pbm", "pgm", "ppm", "pnm", "pfm",
        "pam", "sct", "tga", "vda", "icb", "vst"
    ];

        public BitmapSource LoadImage(string path)
        {
            using var image = new MagickImage(path);
            image.ColorType = ColorType.TrueColorAlpha;
            image.Depth = 16;
            return CreateBitmapSource(image);
        }

        private static BitmapSource CreateBitmapSource(MagickImage image)
        {
            // 直接获取像素集合（无需参数）
            using var pixels = image.GetPixels();
            var bitmap = new WriteableBitmap(
                (int)image.Width,
                (int)image.Height,
                96, 96,
                PixelFormats.Pbgra32,
                null);

            // 转换像素数据到目标格式
            var byteArray = pixels.ToByteArray("RGBA");

            bitmap.WritePixels(
                new Int32Rect(0, 0, (int)image.Width, (int)image.Height),
                byteArray,
                (int)(image.Width * 4),
                0);

            bitmap.Freeze();
            return bitmap;
        }
    }
}
