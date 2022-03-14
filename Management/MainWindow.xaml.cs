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
            await App.fbwfMgr.RefreshAsync();
        }

        private async void OnBtnClick(object sender, RoutedEventArgs e)
        {
            var cacheSize    = App.fbwfMgr.NextSession.OverlayCacheThreshold;
            var selectedDisk = App.fbwfMgr.SelectedDriverLetter;
            var diskLetter   = selectedDisk.Substring(0, 1);

            await App.fbwfMgr.RefreshAsync();

            if (cacheSize != App.fbwfMgr.NextSession.OverlayCacheThreshold)
            {
                App.fbwfMgr.NextSession.OverlayCacheThreshold = cacheSize;
                await FbwfMgr.SetCacheSizeAsync((int)cacheSize);
            }

            if (!VolumeHelper.Exists(selectedDisk) && 
                VolumeHelper.Conforms(selectedDisk))
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
                    await VHDMounter.AttachAsync(fi, selectedDisk.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                ///新增選定的控制磁區
                ///新增需要已存在的磁碟區，所以vhd要先mount
                if (!App.fbwfMgr.NextSession.ProtectedVolume.Any(x => x == diskLetter))
                {
                    await FbwfMgr.AddVolumeAsync(diskLetter.FirstOrDefault());
                }
                ///移除非選定的控制磁區
                foreach (var _diskLetter in App.fbwfMgr.NextSession.ProtectedVolume.Except(new[] { diskLetter }))
                {
                    await FbwfMgr.RemoveVolumeAsync(_diskLetter.FirstOrDefault());
                }
            }
        }
    }
}
