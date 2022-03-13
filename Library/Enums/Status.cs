namespace Fbwf.Library.Enums
{
    public enum Status
    {
        /// <summary>
        /// 停用
        /// </summary>
        Disabled,
        /// <summary>
        /// 啟用
        /// </summary>
        Enable
    }

    internal static class StatusExtensions
    {
        public static Status Parse(string str) =>
            str.Contains("enabled") ? 
                Status.Enable :
                Status.Disabled;
    }
}
