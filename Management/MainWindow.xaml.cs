using Fbwf.Library.Helpers;
using Fbwf.Library.Method;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fbwf.Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.fbwfMgr;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.cfg.Save();
        }

        private async void OnCbClick(object sender, RoutedEventArgs e)
        {
            var tag       = (sender as FrameworkElement).Tag.ToString();
            var isChecked = (sender as CheckBox).IsChecked ?? false;

            switch (tag)
            {
                case "FbwfInstall":
                    await FbwfControl.Install(isChecked);
                    break;
                case "FbwfEnable":
                    await FbwfControl.Enable(isChecked);
                    break;
                case "OverlayCacheDataCompression":
                    await FbwfControl.Compression(isChecked);
                    break;
                case "OverlayCachePreAllocation":
                    await FbwfControl.CachePreAllocation(isChecked);
                    break;
                case "SizeDisplay":
                    await FbwfControl.SetSizeDisplay(isChecked);
                    break;
                case "AutoMountAtBoot":
                    await FbwfControl.VhdAutoMount(isChecked);
                    break;
                default:
                    break;
            }
        }

        private async void OnBtnClick(object sender, RoutedEventArgs e)
        {
            var cacheSize = App.fbwfMgr.DisplayVM.OverlayCacheThreshold;
            var mount     = App.fbwfMgr.SelectedDriverLetter;

            await App.fbwfMgr.RefreshAsync();

            if (cacheSize != App.fbwfMgr.DisplayVM.OverlayCacheThreshold)
            {
                await FbwfMgr.SetCacheSizeAsync((int)cacheSize);
            }

            if (!VolumeHelper.Exists(mount))
            {
                try
                {
                    var fi = new FileInfo(AssemblyData.VhdPath);
                    if (fi.Exists)
                    {
                        await VHDMounter.DetachAsync(fi);
                        fi.Delete();
                    }
                        
                    await VHDMounter.CreatVHDAsync(fi);
                    await VHDMounter.AttachAsync(fi, mount.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
