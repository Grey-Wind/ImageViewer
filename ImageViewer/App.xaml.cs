using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 解析命令行参数
            bool useHdri = e.Args.Contains("-HDRI", StringComparer.OrdinalIgnoreCase);
            var filePath = e.Args.FirstOrDefault(arg =>
                !arg.StartsWith('-') && File.Exists(arg)
            );

            // 创建并显示唯一主窗口
            var mainWindow = new MainWindow(useHdri);
            mainWindow.Show();

            // 延迟加载图片（确保窗口已初始化）
            if (filePath != null)
            {
                mainWindow.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        mainWindow.LoadImage(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"加载图片失败: {ex.Message}");
                    }
                }, DispatcherPriority.ContextIdle);
            }
        }
    }
}
