using System.Windows;
using ImageViewer.Core;
using ImageViewer.Magick;
using ImageViewer.Magick.HDRI;
using Microsoft.Win32;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IImageLoader? _imageLoader;

        public MainWindow()
        {
            InitializeComponent();
            _imageLoader = new MagickLoader();
        }

        public MainWindow(bool useHdri = false)
        {
            InitializeComponent();
            switch (useHdri)
            {
                case false:
                    _imageLoader = new MagickLoader();
                    break;
                case true:
                    _imageLoader = new HdriMagickLoader();
                    break;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadImage(dialog.FileName);
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            {
                LoadImage(files[0]);
            }
        }

        internal void LoadImage(string path)
        {
            try
            {
                MainImage.Source = _imageLoader!.LoadImage(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image:\n{ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}