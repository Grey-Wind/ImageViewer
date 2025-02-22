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
            using var image = new MagickImage(path); // 确保 MagickImage 被清理
            return CreateBitmapSource(image);
        }

        private BitmapImage CreateBitmapSource(MagickImage image)
        {
            image.Format = MagickFormat.Png64;  // 设置图像格式
            var bitmap = new BitmapImage();

            using var stream = new MemoryStream(image.ToByteArray());
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // 确保图像在加载时缓存
            bitmap.EndInit();

            // 在不再使用时清除 bitmap 对象
            ClearImageCache(bitmap);

            return bitmap;
        }

        public void ClearImageCache(BitmapImage image)
        {
            // 强制垃圾回收，清除图片对象
            image = null!;  // 解除引用
            GC.Collect();  // 强制进行垃圾回收
            GC.WaitForPendingFinalizers(); // 等待垃圾回收完成
        }
    }
}
