using Fbwf.Library.Helpers;
using Microsoft.Win32;
using System;

namespace Fbwf.Libaray.Method
{
    public class FbwfRegistry
    {
        const string FbwfRegPath = "SYSTEM\\ControlSet001\\services";

        /// <summary>
        /// 註冊Fbwf Registry
        /// 成功安裝回覆true
        /// </summary>
        public static bool InstallFbwfReg()
        {
            //確認是否註冊檔已登入
            if (IsFbwfExists("EnabledOnAllSkus"))
            {
                var status = FbwfMgr.DisplayConfig();
                //當下狀態為關閉
                if (status.Contains("filter state: disabled.")) 
                {
                    //已啟用，但需重開機才會生效
                    if (status.Contains("filter state: enabled.")) return true;
                    FbwfMgr.Enable();
                }
                return true;
            }

            try
            {
                var hklm   = Registry.LocalMachine;
                var subDir = hklm.OpenSubKey(FbwfRegPath, true);
                var aimDir = subDir.CreateSubKey("Fbwf");
                aimDir.SetValue("EnabledOnAllSkus", (byte)1, RegistryValueKind.DWord);
                aimDir.SetValue("DebugFlags", (byte)0, RegistryValueKind.DWord);
                aimDir.SetValue("DisplayName", "");
                aimDir.SetValue("ErrorControl", (byte)1, RegistryValueKind.DWord);
                aimDir.SetValue("Group", "FSFilter System Recovery");
                aimDir.SetValue("ImagePath", new string[] { "system32\\drivers\\fbwf.sys" });
                aimDir.SetValue("Start", (byte)0, RegistryValueKind.DWord);
                aimDir.SetValue("Tag", (byte)2, RegistryValueKind.DWord);
                aimDir.SetValue("Type", (byte)2, RegistryValueKind.DWord);
                aimDir.SetValue("DependOnService", new string[] { "FltMgr" });

                subDir = hklm.OpenSubKey(FbwfRegPath + "\\Fbwf", true);
                aimDir = subDir.CreateSubKey("Instances");
                aimDir.SetValue("DefaultInstance", "Fbwf Instance");

                subDir = hklm.OpenSubKey(FbwfRegPath + "\\Fbwf\\Instances", true);
                aimDir = subDir.CreateSubKey("Fbwf Instance");
                aimDir.SetValue("Altitude", "226000");
                aimDir.SetValue("Flags", (byte)0, RegistryValueKind.DWord);
                aimDir.Close();
            }
            catch (Exception)
            {
                throw;
            }

            FbwfMgr.Enable();
            return true;
        }


        public static void UninstallFbwfReg()
        {
            var hklm = Registry.LocalMachine;
            var subDir = hklm.OpenSubKey(FbwfRegPath, true);
            if (subDir == null) return;
            try
            {
                subDir.DeleteSubKeyTree("Fbwf");
            }
            catch {}
            subDir.Close();
        }

        /// <summary>
        /// 檢查是Fbwf是否已安裝
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsFbwfExists(string name)
        {
            var hkml     = Registry.LocalMachine;
            var software = hkml.OpenSubKey(FbwfRegPath, true);
            var aimdir   = software.OpenSubKey("Fbwf", true);
            try
            {
                var subkeyNames = aimdir?.GetValueNames();
                if (subkeyNames == null) return false;
                aimdir.Close();

                foreach (string keyName in subkeyNames)
                {
                    if (keyName == name) return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }
    }
}
