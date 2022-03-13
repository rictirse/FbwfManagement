using System;
using System.Runtime.InteropServices;

namespace Fbwf.Library.ViewModel
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MemoryStatusVM
    {
        public uint  dwLength;
        public uint  dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        /// <summary>
        /// 剩餘記憶體容量(MB)
        /// </summary>
        public int FreePhysicalMemory => (int)(Convert.ToInt64(ullAvailPhys) / 1024 / 1024);
        /// <summary>
        /// 總記憶體容量(MB)
        /// </summary>
        public int TotalPhysicalMemory => (int)(Convert.ToInt64(ullTotalPhys) / 1024 / 1024);

        public MemoryStatusVM()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusVM));
        }
    }
}
