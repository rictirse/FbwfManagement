using Fbwf.Library.Method;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Tester
{
    public class VhdTester
    {
        string vhdPath;
        FileInfo fiVhdPath;

        [SetUp]
        public void Setup()
        {
            fiVhdPath = new FileInfo(AssemblyData.VhdPath);
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
                fiVhdPath.Refresh();
                Assert.IsTrue(fiVhdPath.Exists);
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
                await VHDMounter.AttachAsync(fiVhdPath);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task FirstAttachAsync()
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