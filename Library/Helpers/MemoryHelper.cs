using Fbwf.Library.ViewModel;
using System.Runtime.InteropServices;


namespace Fbwf.Library.Helpers
{
    public static class MemoryHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusVM lpBuffer);

        public static MemoryStatusVM MemStatus()
        {
            var MemStatus = new MemoryStatusVM();
            GlobalMemoryStatusEx(MemStatus);

            return MemStatus;
        }

        /// <summary>
        /// 合理且可以使用的記憶體大小(MB)
        /// </summary>
        public static bool CanUseMemSize(float? memSize)
        {
            if (memSize == null || memSize <= 0) return false;
            var MemStatus = new MemoryStatusVM();
            GlobalMemoryStatusEx(MemStatus);

            return MemStatus.TotalPhysicalMemory > memSize;
        }
    }
}
