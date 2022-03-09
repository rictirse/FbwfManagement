using Fbwf.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fbwf.Library.Method
{
    public static class FbwfInstall
    {
        static readonly string InstallPath = Environment.SystemDirectory;
        static List<FileInfo> FbwfFiles => new List<FileInfo>
        {
            new FileInfo(Path.Combine(InstallPath, "drivers", "fbwf.sys")),
            new FileInfo(Path.Combine(InstallPath, "fbwfcfg.dll")),
            new FileInfo(Path.Combine(InstallPath, "fbwfcfg.exe")),
            new FileInfo(Path.Combine(InstallPath, "fbwflib.dll")),
            new FileInfo(Path.Combine(InstallPath, "fbwfMgr.exe"))
        };

        static void BytesToFile(FileInfo fi)
        {
            fi.Refresh();
            if (fi.Exists) return;
            if (!fi.Directory.Exists) fi.Directory.Create();

            File.WriteAllBytes(fi.FullName, fi.Name.ResourceToByteArray());
        }

        /// <summary>
        /// 安裝Fbwf驅動
        /// </summary>
        public static bool Install()
        {
            if (Exists()) 
            {
                FbwfRegistry.InstallFbwfReg();
                return true;
            }
                
            bool status = true;

            try
            {
                FbwfFiles.ForEach(x =>
                {
                    BytesToFile(x);
                    x.Refresh();
                    status &= x.Exists;
                });
            }
            catch
            {
                return false;
            }
            return FbwfRegistry.InstallFbwfReg();
        }

        /// <summary>
        /// Fbwf檔案是否存在
        /// </summary>
        public static bool Exists()
        {
            var status = true;
            FbwfFiles.ForEach(x =>
            {
                x.Refresh();
                status &= x.Exists;
            });
            return status;
        }

        /// <summary>
        /// 移除Fbwf
        /// </summary>
        public static bool Uninstall()
        {
            FbwfMgr.Disable();
            bool status = true;
            try
            {
                FbwfFiles.ForEach(x =>
                {
                    File.Delete(x.FullName);
                    x.Refresh();
                    status &= !x.Exists;
                });
            }
            catch
            {
                return false;
            }
            FbwfRegistry.UninstallFbwfReg();
            return status;
        }
    }
}
