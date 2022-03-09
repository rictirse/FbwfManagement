using Fbwf.Library.Method;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fbwf.Service
{
    public class MountVhd : BackgroundService
    {
        public MountVhd()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var diksLetter = 'F';
                var di = new DirectoryInfo($"{diksLetter}:\\");
                if (!di.Exists)
                { 
                    var vhdInfo = new FileInfo(AssemblyData.VhdPath);
                    if (!vhdInfo.Exists)
                    {
                        await VHDMounter.CreatVHDAsync(vhdInfo);
                        await VHDMounter.AttachAsync(vhdInfo, diksLetter);
                    }
                    else
                    {
                        await VHDMounter.AttachAsync(vhdInfo);
                    }
                }
                Thread.Sleep(60 * 1000);
            }
        }
    }
}
