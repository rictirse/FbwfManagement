using Fbwf.Base;
using Fbwf.Library.Base;
using Fbwf.Library.Enums;
using Fbwf.Library.Helpers;
using Fbwf.Library.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbwf.Library.Method
{
    public partial class FbwfMgr : PropertyBase
    {
        #region display
        /// <summary>
        /// 快取大小
        /// </summary>
        public float? CacheSize
        {
            get => _CacheSize;
            set { SetProperty(ref _CacheSize, value); }
        }
        float? _CacheSize = null;
        /// <summary>
        /// 已選擇的Ramdisk磁碟代號
        /// </summary>
        public string SelectedDriverLetter
        {
            get => _SelectedDriverLetter;
            set { SetProperty(ref _SelectedDriverLetter, value); }
        }
        string _SelectedDriverLetter = null;
        /// <summary>
        /// 能使用的磁碟代號
        /// </summary>
        public ObservableRangeCollection<string> CanUseDriverLetter
        {
            get => _CanUseDriverLetter;
            set { SetProperty(ref _CanUseDriverLetter, value); }
        }
        ObservableRangeCollection<string> _CanUseDriverLetter;
        #endregion
        /// <summary>
        /// Fbwf是否已安裝
        /// </summary>
        public bool IsInstall
        {
            get => _IsInstall;
            set { SetProperty(ref _IsInstall, value); }
        }
        bool _IsInstall = false;
        /// <summary>
        /// 是否需要重開機
        /// </summary>
        public bool NeedReboot
        {
            get => _NeedReboot;
            set { SetProperty(ref _NeedReboot, value); }
        }
        bool _NeedReboot = false;
        /// <summary>
        /// 是否需要重開機
        /// </summary>
        public bool AutoMountAtBoot
        {
            get => _AutoMountAtBoot;
            set { SetProperty(ref _AutoMountAtBoot, value); }
        }
        bool _AutoMountAtBoot = false;
        /// <summary>
        /// 程式標題
        /// </summary>
        public string Title
        {
            get => _Title;
            private set { SetProperty(ref _Title, value); }
        }
        string _Title = Language.Lang.Find("FbwfRamDisk");
        /// <summary>
        /// 目前Fbwf狀態(已生效狀態)
        /// </summary>
        public FbwfStatusVM CurrentSession { get; init; }
        /// <summary>
        /// 修改後的Fbwf狀態
        /// </summary>
        public FbwfStatusVM NextSession { get; init; }
        /// <summary>
        /// 顯示用的VM
        /// init先用 CurrentSession
        /// 有任何修改後改為 NextSession
        /// </summary>
        public FbwfStatusVM DisplayVM
        {
            get => _DisplayVM;
            set => SetProperty(ref _DisplayVM, value);
        }
        FbwfStatusVM _DisplayVM = null;

        private Dictionary<FbwfStatusSession, FbwfStatusVM> FbwfStatus { get; init; }

        public FbwfMgr()
        {
            CurrentSession = new ();
            NextSession    = new ();
            FbwfStatus     = new ();
            FbwfStatus.Add(FbwfStatusSession.Current, CurrentSession);
            FbwfStatus.Add(FbwfStatusSession.Next, NextSession);
            Refresh();
            DisplayVM = CurrentSession;
            this.CanUseDriverLetter = new();
            CanUseDriverLetter.AddRange(VolumeHelper.CanUseDeviceName());
            SelectedDriverLetter = $"{CurrentSession.ProtectedVolume.FirstOrDefault()}\\";
        }

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
            AutoMountAtBoot = FbwfTaskScheduler.Exists();
        }
    }
}
