using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbwf.Library.Helpers
{
    public class VolumeHelper
    {
        /// <summary>
        /// 列出全部已使用磁碟代號
        /// </summary>
        public static IEnumerable<string> AllDevice() =>
            DriveInfo.GetDrives()
                .Select(x => x.Name)
                .ToList();

        /// <summary>
        /// 確認磁碟代號是否已存在
        /// </summary>
        /// <param name="volumeName">可接受單一個英文字代號或磁碟代號後加上:\</param>
        public static bool Exists(string volumeName) =>
            DriveInfo.GetDrives()
                .Select(x => x.Name.Replace($":\\", null))
                .Any(x => x == volumeName?.Replace($":\\", null));

        /// <summary>
        /// 能使用的磁碟代號
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> CanUseDeviceName() =>
            DiskLetterList.Except(AllDevice());
        
        /// <summary>
        /// 是否合法磁碟代號
        /// </summary>
        public static bool Conforms(string diskLetter) =>
            DiskLetterList.Any(x => x == diskLetter);

        static readonly IEnumerable<string> DiskLetterList =
            new[] {"A:\\", "B:\\", "C:\\", "D:\\", "E:\\", "F:\\", "G:\\", "H:\\", "I:\\", "J:\\",
                   "K:\\", "L:\\", "M:\\", "N:\\", "O:\\", "P:\\", "Q:\\", "R:\\", "S:\\", "T:\\",
                   "U:\\", "V:\\", "W:\\", "X:\\", "Y:\\", "Z:\\" };
    }
}