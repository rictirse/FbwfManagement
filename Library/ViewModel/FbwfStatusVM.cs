using Fbwf.Base;
using Fbwf.Library.Base;
using Fbwf.Library.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fbwf.Library.ViewModel
{
    public class FbwfStatusVM : PropertyBase
    {
        /// <summary>
        /// Fbwf是否啟用
        /// </summary>
        public Status Status
        {
            get => _Status;
            set { SetProperty(ref _Status, value); }
        }
        Status _Status = Status.Disabled;

        public Status OverlayCacheDataCompression
        {
            get => _OverlayCacheDataCompression;
            set { SetProperty(ref _OverlayCacheDataCompression, value); }
        }
        Status _OverlayCacheDataCompression = Status.Disabled;

        public Status OverlayCachePreAllocation
        {
            get => _OverlayCachePreAllocation;
            set { SetProperty(ref _OverlayCachePreAllocation, value); }
        }
        Status _OverlayCachePreAllocation = Status.Disabled;
        /// <summary>
        /// 顯示模式
        /// </summary>
        public SizeDisplayMode SizeDisplay
        {
            get => _SizeDisplay;
            set { SetProperty(ref _SizeDisplay, value); }
        }
        SizeDisplayMode _SizeDisplay = SizeDisplayMode.Nominal;
        /// <summary>
        /// 快取大小 (單位GB)
        /// </summary>
        public float OverlayCacheThreshold
        {
            get => _OverlayCacheThreshold;
            set { SetProperty(ref _OverlayCacheThreshold, value); }
        }
        float _OverlayCacheThreshold = 0f;
        /// <summary>
        /// 受控制磁碟區域
        /// </summary>
        public ObservableRangeCollection<string> ProtectedVolume
        {
            get => _ProtectedVolume;
            set => SetProperty(ref _ProtectedVolume, value);
        }
        ObservableRangeCollection<string> _ProtectedVolume = new();
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
        /// 排除磁碟區域
        /// </summary>
        public ObservableRangeCollection<string> WriteThroughListOfEachProtectedVolume
        {
            get => _WriteThroughListOfEachProtectedVolume;
            set => SetProperty(ref _WriteThroughListOfEachProtectedVolume, value);
        }
        ObservableRangeCollection<string> _WriteThroughListOfEachProtectedVolume = new();
        /// <summary>
        /// 沒有磁碟代號的受控制區域
        /// </summary>
        public List<string> LostDiskLetterProtectedVolume { get; set; } = new List<string>();
        /// <summary>
        /// 沒有磁碟代號的排除制區域
        /// </summary>
        public List<string> LostDiskLetterWriteThroughListOfEachProtectedVolume { get; set; } = new List<string>();
        /// <summary>
        /// 清空已加入的控制區與排除區
        /// </summary>
        public void Clear()
        {
            ProtectedVolume.Clear();
            WriteThroughListOfEachProtectedVolume.Clear();
            LostDiskLetterProtectedVolume.Clear();
            LostDiskLetterWriteThroughListOfEachProtectedVolume.Clear();
        }

        public override string ToString()
        {
            var result = $"Fbwf state: {Status}";

            if (OverlayCacheDataCompression == Status.Enable)
            {
                result += ", overlay cache data compression state: Enable";
            }

            result += $", overlay cache threshol: {OverlayCacheThreshold:F4}";
            if (OverlayCachePreAllocation == Status.Enable)
            {
                result += ", overlay cache pre-allocation: is Enable";
            }

            result += $", size display: {SizeDisplay}";

            if (ProtectedVolume.Any())
            {
                result += $", Protected volume list: {string.Join(",", ProtectedVolume)}";
            }

            if (WriteThroughListOfEachProtectedVolume.Any())
            {
                result += $", Write through list of each protected volume: {string.Join(",", ProtectedVolume)}";
            }

            return result;
        }
    }
    internal static class FbwfStatusVMExtensions
    {
        /// <summary>
        /// 確認是否需要重開機
        /// </summary>
        public static bool CheckNeedReboot(this FbwfStatusVM curr, FbwfStatusVM next)
        {
            if (curr.Status != next.Status) return true;
            if (curr.OverlayCacheDataCompression != next.OverlayCacheDataCompression) return true;
            if (curr.OverlayCachePreAllocation != next.OverlayCachePreAllocation) return true;
            if (curr.SizeDisplay != next.SizeDisplay) return true;
            if (curr.OverlayCacheThreshold != next.OverlayCacheThreshold) return true;

            if (!curr.ProtectedVolume
                .All(x => next.ProtectedVolume.Contains(x))) return true;
            if (!curr.WriteThroughListOfEachProtectedVolume
                .All(x => next.WriteThroughListOfEachProtectedVolume.Contains(x))) return true;

            return false;
        }
    }
}