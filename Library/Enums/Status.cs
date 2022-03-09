namespace Fbwf.Library.Enums
{
    public enum Status
    {
        Disabled,
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
