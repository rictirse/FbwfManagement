﻿using Fbwf.Base;
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
        #region Property
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
        /// 能使用的磁碟代號
        /// </summary>
        public ObservableRangeCollection<string> CanUseDriverLetter
        {
            get => _CanUseDriverLetter;
            set { SetProperty(ref _CanUseDriverLetter, value); }
        }
        ObservableRangeCollection<string> _CanUseDriverLetter;
        /// <summary>
        /// Visibility current Fbwf status textblock
        /// </summary>
        public bool FbwfStatusVisibility
        {
            get => _FbwfStatusVisibility;
            set { SetProperty(ref _FbwfStatusVisibility, value); }
        }
        bool _FbwfStatusVisibility = false;
        /// <summary>
        /// Visibility current overlay cache data compression status textblock
        /// </summary>
        public bool OverlayCacheDataCompressionVisibility
        {
            get => _OverlayCacheDataCompressionVisibility;
            set { SetProperty(ref _OverlayCacheDataCompressionVisibility, value); }
        }
        bool _OverlayCacheDataCompressionVisibility = false;
        /// <summary>
        /// Visibility current Overlay cache pre-allocation status textblock
        /// </summary>
        public bool OverlayCachePreAllocationVisibility
        {
            get => _OverlayCachePreAllocationVisibility;
            set { SetProperty(ref _OverlayCachePreAllocationVisibility, value); }
        }
        bool _OverlayCachePreAllocationVisibility = false;
        /// <summary>
        /// Visibility Size display mode textblock
        /// </summary>
        public bool SizeDisplayVisibility
        {
            get => _SizeDisplayVisibility;
            set { SetProperty(ref _SizeDisplayVisibility, value); }
        }
        bool _SizeDisplayVisibility = false;
        /// <summary>
        /// Visibility Overlay cache threshold size textblock
        /// </summary>
        public bool OverlayCacheThresholdVisibility
        {
            get => _OverlayCacheThresholdVisibility;
            set { SetProperty(ref _OverlayCacheThresholdVisibility, value); }
        }
        bool _OverlayCacheThresholdVisibility = false;
        /// <summary>
        /// Visibility Overlay cache threshold size textblock
        /// </summary>
        public bool ProtectedVolumeVisibility
        {
            get => _ProtectedVolumeVisibility;
            set { SetProperty(ref _ProtectedVolumeVisibility, value); }
        }
        bool _ProtectedVolumeVisibility = false;
        /// <summary>
        /// Fbwf是否已安裝
        /// </summary>
        public bool IsInstall
        {
            get => _IsInstall;
            set 
            { 
                SetProperty(ref _IsInstall, value);
                if (!value)
                {
                    CollapsedAll();
                }
            }
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

        private Dictionary<FbwfStatusSession, FbwfStatusVM> FbwfStatus { get; init; }
        #endregion

        public FbwfMgr()
        {
            CurrentSession = new ();
            NextSession    = new ();
            FbwfStatus     = new ();
            FbwfStatus.Add(FbwfStatusSession.Current, CurrentSession);
            FbwfStatus.Add(FbwfStatusSession.Next, NextSession);
            Refresh();
            this.CanUseDriverLetter = new();
            var diskLetter = VolumeHelper.CanUseDeviceName()
                .Union(CurrentSession.ProtectedVolume.Select(x => $"{x}\\"))
                .Union(CurrentSession.ProtectedVolume.Select(x => $"{x}\\"))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            CanUseDriverLetter.AddRange(diskLetter);
        }

        private IEnumerable<string> LostDiskLetterProtectedVolume =>
             CurrentSession.LostDiskLetterProtectedVolume
                .Union(NextSession.LostDiskLetterProtectedVolume)
                .ToList();

        /// <summary>
        /// 刪除沒有磁碟代號的fbwf受控區
        /// </summary>
        public void RemoveLostDiskLetterVolume()
        {
            foreach (var volume in LostDiskLetterProtectedVolume)
            {
                RemoveLostDiskLetterVolume(volume);
            }
        }

        /// <summary>
        /// 刪除沒有磁碟代號的fbwf受控區
        /// </summary>
        public async Task RemoveLostDiskLetterVolumeAsync()
        {
            foreach (var volume in LostDiskLetterProtectedVolume)
            {
                await RemoveLostDiskLetterVolumeAsync(volume);
            }
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
                    currentStatus.Clear();
                    IsInstall  = true;
                    volumeType = VolumeType.Other;
                }
                else if (line.Contains("File-based write filter configuration for the next session:"))
                {
                    currentStatus = FbwfStatus[FbwfStatusSession.Next];
                    currentStatus.Clear();
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
                    if (sp[1].ToLower().Contains("gb"))
                    {
                        value *= 1000;
                    }
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
                    if (sp.Length == 1 && 
                        !line.ToLower().Contains("exit"))
                    {
                        if (volumeType == VolumeType.ProtectedVolume &&
                            !currentStatus.LostDiskLetterProtectedVolume.Any(x => x == line))
                        {
                            currentStatus.LostDiskLetterProtectedVolume.Add(line);
                        }
                        else if (volumeType == VolumeType.WriteThroughListOfEachProtectedVolume &&
                                !currentStatus.LostDiskLetterWriteThroughListOfEachProtectedVolume.Any(x => x == line))
                        {
                            currentStatus.LostDiskLetterWriteThroughListOfEachProtectedVolume.Add(line);
                        }
                    }
                    else if (sp.Length == 2)
                    {
                        var volume = sp[1].Remove(sp[1].Length - 1, 1);
                       
                        if (volumeType == VolumeType.ProtectedVolume &&
                            !currentStatus.ProtectedVolume.Any(x => x == volume))
                        {
                            currentStatus.ProtectedVolume.Add(volume);
                        }
                        else if (volumeType == VolumeType.WriteThroughListOfEachProtectedVolume)
                        {
                            //未實作
                        }
                    }
                }
            }
            UpdateProperty();
        }

        /// <summary>
        /// 更新UI上的顯示
        /// </summary>
        private void UpdateProperty()
        {
            var nextProtectedVolume = NextSession.ProtectedVolume.FirstOrDefault();
            if (!string.IsNullOrEmpty(nextProtectedVolume))
            { 
                NextSession.SelectedDriverLetter = $"{nextProtectedVolume}\\";
            }

            FbwfStatusVisibility = 
                CurrentSession.Status != NextSession.Status;
            OverlayCacheDataCompressionVisibility =
                CurrentSession.OverlayCacheDataCompression != NextSession.OverlayCacheDataCompression;
            OverlayCachePreAllocationVisibility =
                CurrentSession.OverlayCachePreAllocation != NextSession.OverlayCachePreAllocation;
            SizeDisplayVisibility =
                CurrentSession.SizeDisplay != NextSession.SizeDisplay;
            OverlayCacheThresholdVisibility = 
                CurrentSession.OverlayCacheThreshold != NextSession.OverlayCacheThreshold;
            ProtectedVolumeVisibility = 
                string.Join(",", CurrentSession.ProtectedVolume) != string.Join(",", NextSession.ProtectedVolume);

            NeedReboot = CurrentSession.CheckNeedReboot(NextSession);
            AutoMountAtBoot = FbwfTaskScheduler.Exists();
        }

        private void CollapsedAll()
        {
            FbwfStatusVisibility = false;
            OverlayCacheDataCompressionVisibility = false;
            OverlayCachePreAllocationVisibility = false;
            SizeDisplayVisibility = false;
            OverlayCacheThresholdVisibility = false;
            ProtectedVolumeVisibility = false;
            AutoMountAtBoot = false;
        }
    }
}
