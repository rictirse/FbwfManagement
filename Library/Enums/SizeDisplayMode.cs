namespace Fbwf.Library.Enums
{
    public enum SizeDisplayMode
    {
        /// <summary>
        /// 顯示硬碟容量模式
        /// </summary>
        Nominal,
        /// <summary>
        /// 顯示快取記憶體大小模式
        /// </summary>
        Virtual,
    }

    internal static class SizeDisplayExtensions
    {
        public static SizeDisplayMode Parse(string str) =>
            str.Contains("virtual mode.") ? 
                SizeDisplayMode.Virtual : 
                SizeDisplayMode.Nominal;
    }
}
