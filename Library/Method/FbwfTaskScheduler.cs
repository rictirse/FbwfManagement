using Fbwf.Library.Helpers;
using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Linq;
using System.Security.Principal;

namespace Fbwf.Library.Method
{
    public class FbwfTaskScheduler
    {
        const string TaskSchedulerName = "FbwfVhdAutoMount";

        static string GetMountScript => $"select vdisk file=\"{AssemblyData.VhdPath}\"\r\nattach vdisk";

        /// <summary>
        /// 寫入vhd自動掛載腳本
        /// </summary>
        public static void MountScriptWriteToFile() =>
            File.WriteAllText(AssemblyData.MountScript, GetMountScript);

        /// <summary>
        /// 寫入vhd自動掛載腳本
        /// </summary>
        public static async System.Threading.Tasks.Task MountScriptWriteToFileAsync() =>
           await File.WriteAllTextAsync(AssemblyData.MountScript, GetMountScript);

        /// <summary>
        /// 確認排程是否存在
        /// </summary>
        public static bool Exists()
        {
            using (var ts = new TaskService(""))
            {
                using (var tf = ts.GetFolder(""))
                {
                    return tf.Tasks.Any(x => x.Name == TaskSchedulerName);
                }
            }
        }

        /// <summary>
        /// 寫入排程
        /// </summary>
        public static void Write()
        {
            UAC.Check();

            using (var ts = new TaskService(""))
            {
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "Attach vDisk";

                td.Triggers.Add(new LogonTrigger());

                td.Principal.UserId = "SYSTEM";
                td.Principal.RunLevel = TaskRunLevel.Highest;

                td.Actions.Add(new ExecAction("diskpart.exe", GetMountScript, null));

                ts.RootFolder.RegisterTaskDefinition(TaskSchedulerName, td);
            }
        }

        /// <summary>
        /// 刪除排程
        /// </summary>
        public static void Delete()
        {
            UAC.Check();

            using (var ts = new TaskService(""))
            {
                var t = ts.GetTask(TaskSchedulerName);
                if (t == null) return;
                t.Definition.Triggers.First().StartBoundary = DateTime.Today + TimeSpan.FromDays(7);
                t.RegisterChanges();

                ts.RootFolder.DeleteTask(TaskSchedulerName);
            }
        }
    }
}
