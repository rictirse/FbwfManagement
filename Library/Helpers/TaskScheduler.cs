using Microsoft.Win32.TaskScheduler;
using System;
using System.Linq;
using System.Security.Principal;

namespace Fbwf.Library.Helpers
{
    internal class TaskScheduler
    {
        public static bool ExistsScheduler()
        {
            using (var ts = new TaskService(""))
            {
                using (var tf = ts.GetFolder(""))
                {
                    return tf.Tasks.Any(x => x.Name == "AutoMount");
                }
            }
        }

        public static void WriteScheduler()
        {
            using (var ts = new TaskService(""))
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Attach vDisk";

                td.Triggers.Add(new LogonTrigger());

                td.Principal.UserId = "SYSTEM";
                td.Principal.RunLevel = TaskRunLevel.Highest;

                td.Actions.Add(new ExecAction("diskpart.exe", @"/s """ + Environment.CurrentDirectory + @"\\AutoMount.txt""", null));

                ts.RootFolder.RegisterTaskDefinition(@"AutoMount", td);
            }
        }

        public static void DeleteTask()
        {
            using (var ts = new TaskService(""))
            {
                var t = ts.GetTask("AutoMount");
                if (t == null) return;
                t.Definition.Triggers[0].StartBoundary = DateTime.Today + TimeSpan.FromDays(7);
                t.RegisterChanges();

                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                    throw new Exception($"Cannot delete task with your current identity '{identity.Name}' permissions level." +
                    "You likely need to run this application 'as administrator' even if you are using an administrator account.");

                ts.RootFolder.DeleteTask("AutoMount");
            }
        }
    }
}
