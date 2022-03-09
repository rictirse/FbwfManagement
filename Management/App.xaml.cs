using Fbwf.Library.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Fbwf.Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static XConfig cfg { get; private set; }

        public App()
        {
            //FbwfInstall.Install();
            //var result = FbwfMgrHelper.Command("DisplayConfig");
            cfg = new();
            cfg.Load();
        }
    }
}
