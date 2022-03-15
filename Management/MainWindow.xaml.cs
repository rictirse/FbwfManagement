using Fbwf.Library.Helpers;
using Fbwf.Library.Method;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            var fbwfMgr         = App.fbwfMgr.NextSession;
            var cacheSize       = fbwfMgr.OverlayCacheThreshold;
            var selectedDisk    = fbwfMgr.SelectedDriverLetter;
            var diskLetter_str  = selectedDisk.Substring(0, 1);
            var diskLetter_char = selectedDisk.FirstOrDefault();

            if (!VolumeHelper.Conforms(diskLetter_char))
            {
                MessageBox.Show("Please specify a valid drive.", "", MessageBoxButton.OK);
                return;
            }
            if (cacheSize <= 0)
            {
                MessageBox.Show("Please enter a valid cache size.", "", MessageBoxButton.OK);
                return;
            }

            await App.fbwfMgr.RefreshAsync();

            if (cacheSize != fbwfMgr.OverlayCacheThreshold)
            {
                fbwfMgr.OverlayCacheThreshold = cacheSize;
                await FbwfMgr.SetCacheSizeAsync((int)cacheSize);
            }

            if (!VolumeHelper.Exists(diskLetter_char))
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
                    await VHDMounter.AttachAsync(fi, diskLetter_char);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                ///新增選定的控制磁區
                ///新增需要已存在的磁碟區，所以vhd要先mount
                if (!fbwfMgr.ProtectedVolume.Any(x => x == $"{diskLetter_str}:"))
                {
                    await FbwfMgr.AddVolumeAsync(diskLetter_char);
                }
                ///移除非選定的控制磁區
                foreach (var _diskLetter in fbwfMgr.ProtectedVolume.Except(new[] { $"{diskLetter_str}:" }))
                {
                    await FbwfMgr.RemoveVolumeAsync(diskLetter_char);
                }
            }
            await App.fbwfMgr.RefreshAsync();
            MessageBox.Show("Requires a restart to finish setting.", "", MessageBoxButton.OK);
        }
    }
}
