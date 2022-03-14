using Fbwf.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        static async Task BytesToFileAsync(FileInfo fi)
        {
            fi.Refresh();
            if (fi.Exists) return;
            if (!fi.Directory.Exists) fi.Directory.Create();

            await File.WriteAllBytesAsync(fi.FullName, fi.Name.ResourceToByteArray());
        }

        #region Install
        /// <summary>
        /// 安裝Fbwf驅動
        /// </summary>
        public static bool Install()
        {
            if (Exists()) 
            {
                FbwfRegistry.Write();
                return true;
            }
            UAC.Check();
                
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
            status &= FbwfRegistry.Write();
            return status;
        }

        /// <summary>
        /// 安裝Fbwf驅動
        /// </summary>
        public static async Task<bool> InstallAsync()
        {
            if (Exists())
            {
                FbwfRegistry.Write();
                return true;
            }
            UAC.Check();

            bool status = true;

            try
            {
                foreach (var fi in FbwfFiles)
                {
                    await BytesToFileAsync(fi);
                    fi.Refresh();
                    status &= fi.Exists;
                }
            }
            catch
            {
                return false;
            }

            status &= FbwfRegistry.Write();
            return status;
        }
        #endregion

        /// <summary>
        /// 移除Fbwf
        /// </summary>
        public static bool Uninstall()
        {
            UAC.Check();
            FbwfMgr.Disable();
            FbwfTaskScheduler.Delete();
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

            FbwfRegistry.Delete();
            return status;
        }

        /// <summary>
        /// 移除Fbwf
        /// </summary>
        public static async Task<bool> UninstallAsync()
        {
            UAC.Check();
            await FbwfMgr.DisableAsync();
            FbwfTaskScheduler.Delete();
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
            FbwfRegistry.Delete();
            return status;
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
    }
}
