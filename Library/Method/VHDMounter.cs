using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Library.Helpers
{
    public static class VHDMounter
    {
        /// <summary>
        /// 建立VHD
        /// </summary>
        /// <param name="vDiskPath">vhd建立位置</param>
        /// <param name="size">vhd大小(MB)</param>
        public static void CreatVHD(FileInfo vDiskPath, int size = 32) =>
             Command(CreatVHDCmd(vDiskPath.FullName, size));

        /// <summary>
        /// 建立VHD
        /// </summary>
        /// <param name="vDiskPath">vhd建立位置</param>
        /// <param name="size">vhd大小(MB)</param>
        public static async Task CreatVHDAsync(FileInfo vDiskPath, int size = 32) =>
             await CommandAsync(CreatVHDCmd(vDiskPath.FullName, size));

        static string CreatVHDCmd(string fullName, int size) =>
            $"CREATE VDISK FILE=\"{fullName}\" MAXIMUM={size} TYPE=EXPANDABLE";

        /// <summary>
        /// 建立排程自動掛載VHD腳本
        /// </summary>
        /// <param name="mountScriptPath">腳本路徑</param>
        /// <param name="vDiskPath">VHD路徑</param>
        public static void CreatMountScript(FileInfo mountScriptPath, FileInfo vDiskPath)
        {
            if (mountScriptPath.Exists) return;
            File.WriteAllText(mountScriptPath.FullName, CreatMountScriptCmd(vDiskPath.FullName));
        }

        /// <summary>
        /// 建立排程自動掛載VHD腳本
        /// </summary>
        /// <param name="mountScriptPath">腳本路徑</param>
        /// <param name="vDiskPath">VHD路徑</param>
        public static async Task CreatMountScriptAsync(FileInfo mountScriptPath, FileInfo vDiskPath)
        {
            if (mountScriptPath.Exists) return;
            await File.WriteAllTextAsync(mountScriptPath.FullName, CreatMountScriptCmd(vDiskPath.FullName));
        }

        private static string CreatMountScriptCmd(string fullName) =>
            $"select vdisk file=\"{fullName}\"\r\n" +
            $"attach vdisk";

        /// <summary>
        /// 掛載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        /// <param name="diksLetter">指定掛載的磁碟代號</param>
        public static void Attach(FileInfo vDiskPath, char diksLetter)
        {
            if (char.IsWhiteSpace(diksLetter)) return;
            Command(AttachCmd(vDiskPath.FullName, diksLetter));
        }

        /// <summary>
        /// 掛載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        /// <param name="diksLetter">指定掛載的磁碟代號</param>
        public static async Task AttachAsync(FileInfo vDiskPath, char diksLetter)
        {
            if (char.IsWhiteSpace(diksLetter)) return;
            await CommandAsync(AttachCmd(vDiskPath.FullName, diksLetter));
        }

        private static string[] AttachCmd(string fullName, char diksLetter) =>
            new [] 
            {
                $"select vdisk file =\"{fullName}\"",
                "attach vdisk",
                "create partition primary",
                "format fs=ntfs quick",
                $"assign letter={diksLetter}" 
            };

        /// <summary>
        /// 卸載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        public static void Detach(FileInfo vDiskPath) =>
            Command(DetachCmd(vDiskPath.FullName));

        /// <summary>
        /// 卸載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        public static async Task DetachAsync(FileInfo vDiskPath) =>
            await CommandAsync(DetachCmd(vDiskPath.FullName));

        private static string DetachCmd(string fullName) =>
            $"select vdisk file =\"{fullName}\"\r\ndetach vdisk";

        private static string Command(params string[] cmds) =>
            ConsoleHelper.Command("diskpart.exe", cmds);

        private static async Task<string> CommandAsync(params string[] cmds) =>
            await ConsoleHelper.CommandAsync("diskpart.exe", cmds);
    }
}
