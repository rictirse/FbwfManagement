using Fbwf.Library.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Library.Method
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

        /// <summary>
        /// 建立VHD指令
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static string CreatVHDCmd(string fullName, int size) =>
            $"CREATE VDISK FILE=\"{fullName}\" MAXIMUM={size} TYPE=EXPANDABLE";

        /// <summary>
        /// 掛載VHD指令
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private static string[] AttachtCmd(string fullName) =>
            new[] { $"select vdisk file =\"{fullName}\"", "attach vdisk" };

        /// <summary>
        /// 掛載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        public static void Attach(FileInfo vDiskPath) =>
            Command(AttachtCmd(vDiskPath.FullName));
        
        /// <summary>
        /// 掛載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        public static async Task AttachAsync(FileInfo vDiskPath) =>
            await CommandAsync(AttachtCmd(vDiskPath.FullName));
        
        /// <summary>
        /// 初始化 + 掛載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        /// <param name="diksLetter">指定掛載的磁碟代號</param>
        public static void Attach(FileInfo vDiskPath, char diksLetter)
        {
            if (char.IsWhiteSpace(diksLetter)) return;
            Command(FirstAttachCmd(vDiskPath.FullName, diksLetter));
        }

        /// <summary>
        /// 初始化 + 掛載VHD
        /// </summary>
        /// <param name="vDiskPath">VHD路徑</param>
        /// <param name="diksLetter">指定掛載的磁碟代號</param>
        public static async Task AttachAsync(FileInfo vDiskPath, char diksLetter)
        {
            if (char.IsWhiteSpace(diksLetter)) return;
            await CommandAsync(FirstAttachCmd(vDiskPath.FullName, diksLetter));
        }

        /// <summary>
        /// 初次掛載 + 格式化指令
        /// </summary>
        private static string[] FirstAttachCmd(string fullName, char diksLetter) =>
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

        /// <summary>
        /// 卸載VHD指令
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private static string[] DetachCmd(string fullName) =>
            new[] { $"select vdisk file =\"{fullName}\"", "detach vdisk" };

        private static string Command(params string[] cmds) =>
            ConsoleHelper.Command("diskpart.exe", cmds);

        private static async Task<string> CommandAsync(params string[] cmds) =>
            await ConsoleHelper.CommandAsync("diskpart.exe", cmds);
    }
}
