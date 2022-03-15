using Fbwf.Base;
using Fbwf.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbwf.Library.Method
{
    public partial class FbwfMgr : PropertyBase
    {
        #region Enable / Disable
        public static string Disable() =>
            Command("Disable");

        public static async Task<string> DisableAsync() =>
            await CommandAsync("Disable");

        public static string Enable() =>
            Command("Enable");

        public static async Task<string> EnableAsync() =>
            await CommandAsync("Enable");
        #endregion

        #region Volume
        /// <summary>
        /// 新增Fbwf受控制磁碟代號
        /// </summary>
        public static string AddVolume(char diskLetter) =>
            Command($"Addvolume {diskLetter}:");

        /// <summary>
        /// 新增Fbwf受控制磁碟代號
        /// </summary>
        public static async Task<string> AddVolumeAsync(char diskLetter) =>
            await CommandAsync($"Addvolume {diskLetter}:");

        /// <summary>
        /// 移除Fbwf受控制磁碟代號
        /// </summary>
        public static string RemoveVolume(char diskLetter) =>
            Command($"Removevolume {diskLetter}: 1");

        /// <summary>
        /// 移除Fbwf受控制磁碟代號
        /// </summary>
        public static async Task<string> RemoveVolumeAsync(char diskLetter) =>
            await CommandAsync($"Removevolume {diskLetter}: 1");

        /// <summary>
        /// 移除Fbwf遺失磁碟代號的受控區域
        /// </summary>
        public static string RemoveLostDiskLetterVolume(string volumeName) =>
            Command($"Removevolume {volumeName}: 1");

        /// <summary>
        /// 移除Fbwf遺失磁碟代號的受控區域
        /// </summary>
        public static async Task<string> RemoveLostDiskLetterVolumeAsync(string volumeName) =>
            await CommandAsync($"Removevolume {volumeName}: 1");
        #endregion

        #region DisplayConfig
        public static string DisplayConfig() =>
            Command("DisplayConfig");

        public static async Task<string> DisplayConfigAsync() =>
            await CommandAsync("DisplayConfig");
        #endregion

        #region Refresh
        public async Task RefreshAsync() =>
            ParseStatus(await DisplayConfigAsync());

        public void Refresh() =>
            ParseStatus(DisplayConfig());
        #endregion

        #region Compression
        public static string DisableCompression() =>
            Command("Setcompressio 0");

        public static async Task<string> DisableCompressionAsync() =>
            await CommandAsync("Setcompressio 1");

        public static string EnableCompression() =>
            Command("Setcompressio 1");

        public static async Task<string> EnableCompressionAsync() =>
            await CommandAsync("Setcompressio 1");
        #endregion

        #region Pre-Allocation
        public static string DisablePreAllocation() =>
            Command("Setpreallocation 0");

        public static async Task<string> DisablePreAllocationAsync() =>
            await CommandAsync("Setpreallocation 0");

        public static string EnablePreAllocation() =>
            Command("Setpreallocation 1");

        public static async Task<string> EnablePreAllocationAsync() =>
            await CommandAsync("Setpreallocation 1");
        #endregion

        #region SetCacheSize
        /// <summary>
        /// 設定快取大小(MB)
        /// </summary>
        /// <param name="memSize">記憶體大小(MB)</param>
        public static string SetCacheSize(int memSize) =>
            Command($"Setthreshold {memSize}");

        /// <summary>
        /// 設定快取大小(MB)
        /// </summary>
        /// <param name="memSize">記憶體大小(MB)</param>
        public static async Task<string> SetCacheSizeAsync(int memSize) =>
            await CommandAsync($"Setthreshold {memSize}");
        #endregion

        #region SetSizeDisplay
        /// <summary>
        /// 設定顯示模式
        /// </summary>
        public static string SetSizeDisplay(int mode) =>
             Command($"Setsizedisplay {mode}");

        /// <summary>
        /// 設定顯示模式
        /// </summary>
        public static async Task SetSizeDisplayAsync(int mode) =>
             await CommandAsync($"Setsizedisplay {mode}");
        #endregion

        #region Command
        private static string Command(string cmd) =>
            ConsoleHelper.CmdCommand($"fbwfmgr.exe /{cmd}").TrimCmd();

        private static async Task<string> CommandAsync(string cmd) =>
            (await ConsoleHelper.CmdCommandAsync($"fbwfmgr.exe /{cmd}")).TrimCmd();
        #endregion
    }

    public static class FbwfMgrextensions
    {
        internal static string TrimCmd(this string @this)
        {
            var sp = @this.Replace($"{Environment.CurrentDirectory}>", null)
                          .Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            return string.Join("\r\n", sp[2..^1]);
        }
    }
}