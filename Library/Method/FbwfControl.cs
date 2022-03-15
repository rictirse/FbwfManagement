using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fbwf.Library.Method
{
    public static class FbwfControl
    {
        /// <summary>
        /// 安裝Fbwf 
        /// </summary>
        /// <param name="status">true安裝 / false 解除安裝</param>
        public static async Task Install(bool status)
        {
            if (status)
            {
                await FbwfInstall.InstallAsync();
                MessageBox.Show("Requires a restart to finish installing", "", MessageBoxButton.OK);
                Environment.Exit(1);
            }
            else
            {
                FbwfInstall.Uninstall();
            }
        }

        /// <summary>
        /// 啟用Fbwf 
        /// </summary>
        /// <param name="status">true 啟用 / false 停用</param>
        public static async Task Enable(bool status)
        {
            if (status)
            {
                await FbwfMgr.EnableAsync();
            }
            else
            {
                await FbwfMgr.DisableAsync();
            }
        }

        /// <summary>
        /// 壓縮使用的快取記憶體
        /// </summary>
        /// <param name="status">true啟用 / false 停用</param>
        public static async Task Compression(bool status)
        {
            if (status)
            {
                await FbwfMgr.EnableCompressionAsync();
            }
            else
            {
                await FbwfMgr.DisableCompressionAsync();
            }
        }

        /// <summary>
        /// 快取記憶體預載
        /// 啟動:快取記憶體會直接佔用可用記憶體
        /// 停用:快取記憶體為動態分配，用多少吃多少
        /// </summary>
        /// <param name="status">true啟用 / false 停用</param>
        public static async Task CachePreAllocation(bool status)
        {
            if (status)
            {
                await FbwfMgr.EnablePreAllocationAsync();
            }
            else
            {
                await FbwfMgr.DisablePreAllocationAsync();
            }
        }

        /// <summary>
        /// 設定顯示模式
        /// Nominal: 顯示vhd容量 
        /// Virtual: 顯示快取記憶體大小
        /// </summary>
        /// <param name="status">true virtual mode / false nominal mode</param>
        public static async Task SetSizeDisplay(bool status)
        {
            if (status)
            {
                await FbwfMgr.SetSizeDisplayAsync(1);
            }
            else
            {
                await FbwfMgr.SetSizeDisplayAsync(0);
            }
        }

        /// <summary>
        /// 重開機後自動掛載VHD
        /// </summary>
        /// <param name="status">true 掛載 / false 卸載</param>
        /// <returns></returns>
        public static async Task VhdAutoMount(bool status)
        {
            if (status)
            {
                await FbwfTaskScheduler.MountScriptWriteToFileAsync();
                FbwfTaskScheduler.Write();
            }
            else
            {
                FbwfTaskScheduler.Delete();
            }
        }
    }
}
