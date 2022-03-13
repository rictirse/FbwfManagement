using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fbwf.Library.Enums
{
    public enum VolumeType
    {
        /// <summary>
        /// 其他
        /// </summary>
        Other = -1,
        /// <summary>
        /// 受控制的磁碟區
        /// </summary>
        ProtectedVolume,
        /// <summary>
        /// 例外清單
        /// </summary>
        WriteThroughListOfEachProtectedVolume,
    }
}
