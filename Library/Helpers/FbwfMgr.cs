using Fbwf.Base;
using Fbwf.Library.Enums;
using Fbwf.Library.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbwf.Library.Helpers
{
    public class FbwfMgr : PropertyBase
    {
        /// <summary>
        /// 是否已安裝
        /// </summary>
        public bool IsInstall
        {
            get => _IsInstall;
            private set { SetProperty(ref _IsInstall, value); }
        }
        bool _IsInstall = false;
        /// <summary>
        /// 是否需要重開機
        /// </summary>
        public bool NeedReboot
        {
            get => _NeedReboot;
            private set { SetProperty(ref _NeedReboot, value); }
        }
        bool _NeedReboot = false;
        /// <summary>
        /// 目前Fbwf狀態
        /// </summary>
        public FbwfStatusVM CurrentSession { get; init; }
        /// <summary>
        /// 重開機後的Fbwf狀態
        /// </summary>
        public FbwfStatusVM NextSession { get; init; }

        private Dictionary<FbwfStatusSession, FbwfStatusVM> FbwfStatus { get; init; }

        public FbwfMgr()
        {
            CurrentSession = new ();
            NextSession    = new ();
            FbwfStatus     = new ();
            FbwfStatus.Add(FbwfStatusSession.Current, CurrentSession);
            FbwfStatus.Add(FbwfStatusSession.Next, NextSession);
        }

        public static void Disable()
        {
            var result = Command("Disable");
        }

        public static void Enable() =>
            Command("Enable");

        public static string DisplayConfig() =>
            Command("DisplayConfig");

        public static async Task<string> DisplayConfigAsync() =>
            await CommandAsync("DisplayConfig");

        public async Task RefreshAsync() =>
            ParseStatus(await DisplayConfigAsync());
        
        public void Refresh() =>
            ParseStatus(DisplayConfig());

        private void ParseStatus(string status)
        {
            var volumeType = VolumeType.Other;
            FbwfStatusVM currentStatus = null;

            foreach (var line in status.Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains("File-based write filter configuration for the current session:"))
                {
                    currentStatus = FbwfStatus[FbwfStatusSession.Current];
                    IsInstall  = true;
                    volumeType = VolumeType.Other;
                }
                else if (line.Contains("File-based write filter configuration for the next session:"))
                {
                    currentStatus = FbwfStatus[FbwfStatusSession.Next];
                    IsInstall  = true;
                    volumeType = VolumeType.Other;
                }
                else if (line.Contains("filter state:"))
                {
                    currentStatus.Status = StatusExtensions.Parse(line);
                }
                else if (line.Contains("overlay cache data compression state:"))
                {
                    currentStatus.OverlayCacheDataCompression = StatusExtensions.Parse(line);
                }
                else if (line.Contains("overlay cache threshold:"))
                {
                    var sp = line.Split(':');
                    float.TryParse(sp[1].Remove(sp[1].Length - 4, 4), out var value);
                    currentStatus.OverlayCacheThreshold = value;
                }
                else if (line.Contains("overlay cache pre-allocation:"))
                {
                    currentStatus.OverlayCachePreAllocation = StatusExtensions.Parse(line);
                }
                else if (line.Contains("size display:"))
                {
                    currentStatus.SizeDisplay = SizeDisplayExtensions.Parse(line);
                }
                else if (line.Contains("protected volume list:"))
                {
                    volumeType = VolumeType.ProtectedVolume;
                }
                else if (line.Contains("write through list of each protected volume:"))
                {
                    volumeType = VolumeType.WriteThroughListOfEachProtectedVolume;
                }
                else if (volumeType != VolumeType.Other)
                {
                    var sp = line.Split('(');
                    if (sp.Length == 2)
                    {
                        var volume = sp[1].Remove(sp[1].Length - 1, 1);
                        if (volume.Contains("none")) continue;
                        if (volumeType == VolumeType.ProtectedVolume &&
                            !currentStatus.ProtectedVolume.Any(x => x == volume))
                        {
                            currentStatus.ProtectedVolume.Add(volume);
                        }
                        else if (volumeType == VolumeType.WriteThroughListOfEachProtectedVolume &&
                                !currentStatus.WriteThroughListOfEachProtectedVolume.Any(x => x == volume))
                        {
                            currentStatus.WriteThroughListOfEachProtectedVolume.Add(volume);
                        }
                    }
                }
            }
            NeedReboot = CurrentSession.CheckNeedReboot(NextSession);
        }

        /// <summary>
        /// 設定快取大小
        /// </summary>
        /// <param name="memSize">記憶體大小(MB)</param>
        public static void SetCacheSize(int memSize)
        {
            var result = Command($"Setthreshold {memSize}");
        }

        public static void SetSizeDisplay(int mode)
        {
            var result = Command($"Setsizedisplay {mode}");
        }

        private static string Command(string cmd) =>
            ConsoleHelper.CmdCommand("fbwfmgr.exe", cmd);

        private static async Task<string> CommandAsync(string cmd) =>
            await ConsoleHelper.CommandAsync("fbwfmgr.exe", cmd);

    }
}
