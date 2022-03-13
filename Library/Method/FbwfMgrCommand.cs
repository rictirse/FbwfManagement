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
        public static void Disable() =>
            Command("Disable");

        public static async Task DisableAsync() =>
            await CommandAsync("Disable");

        public static void Enable() =>
            Command("Enable");

        public static async Task EnableAsync() =>
            await CommandAsync("Enable");
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

        #region OverlayCacheDataCompression
        public static void DisableOverlayCacheDataCompression() =>
            Command("Enable");

        public static async Task DisableOverlayCacheDataCompressionAsync() =>
            await CommandAsync("Enable");

        public static void EnableOverlayCacheDataCompression() =>
            Command("Enable");

        public static async Task EnableOverlayCacheDataCompressionAsync() =>
            await CommandAsync("Enable");
        #endregion

        #region OverlayCachePreAllocation
        public static void DisableOverlayCachePreAllocation() =>
            Command("Enable");

        public static async Task DisableOverlayCachePreAllocationAsync() =>
            await CommandAsync("Enable");

        public static void EnableOverlayCachePreAllocation() =>
            Command("Enable");

        public static async Task EnableOverlayCachePreAllocationAsync() =>
            await CommandAsync("Enable");
        #endregion

        #region SetCacheSize
        /// <summary>
        /// 設定快取大小(MB)
        /// </summary>
        /// <param name="memSize">記憶體大小(MB)</param>
        public static void SetCacheSize(int memSize) =>
            Command($"Setthreshold {memSize}");

        /// <summary>
        /// 設定快取大小(MB)
        /// </summary>
        /// <param name="memSize">記憶體大小(MB)</param>
        public static async Task SetCacheSizeAsync(int memSize) =>
            await CommandAsync($"Setthreshold {memSize}");
        #endregion

        #region SetSizeDisplay
        /// <summary>
        /// 設定顯示模式
        /// </summary>
        public static void SetSizeDisplay(int mode) =>
             Command($"Setsizedisplay {mode}");

        /// <summary>
        /// 設定顯示模式
        /// </summary>
        public static async Task SetSizeDisplayAsync(int mode) =>
             await CommandAsync($"Setsizedisplay {mode}");
        #endregion

        #region Command
        private static string Command(string cmd) =>
            ConsoleHelper.CmdCommand("fbwfmgr.exe", cmd);

        private static async Task<string> CommandAsync(string cmd) =>
            await ConsoleHelper.CommandAsync("fbwfmgr.exe", cmd);
        #endregion
    }
}