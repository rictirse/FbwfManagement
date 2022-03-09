using Fbwf.Library.Helpers;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Tester
{
    public class VhdTester
    {
        const string vhdPath = "R:\\fbwfVisualDisk.vhd";
        FileInfo fiVhdPath;

        [SetUp]
        public void Setup()
        {
            fiVhdPath = new FileInfo(vhdPath);
        }

        [Test]
        public void CreatVHD()
        {
            try
            {
                VHDMounter.CreatVHD(fiVhdPath);
                Assert.IsTrue(File.Exists(vhdPath));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task CreatVHDAsync()
        {
            try
            {
                await VHDMounter.CreatVHDAsync(fiVhdPath);
                Assert.IsTrue(File.Exists(vhdPath));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task AttachAsync()
        {
            try
            {
                await VHDMounter.AttachAsync(fiVhdPath, 'F');
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task DetachAsync()
        {
            try
            {
                await VHDMounter.DetachAsync(fiVhdPath);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}