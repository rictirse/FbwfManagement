namespace Fbwf.Library.Enums
{
    public enum SizeDisplayMode
    {
        Nominal,
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
