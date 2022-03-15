using Fbwf.Library.Helpers;
using Fbwf.Library.Method;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Tester
{
    internal class DriveTester
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetMemStatus()
        {
            try
            {
                var status = MemoryHelper.MemStatus();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetAllVolume()
        {
            try
            {
                var status = VolumeHelper.AllDevice();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void VolumeExists()
        {
            try
            {
                var status = VolumeHelper.Exists('E');
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void CanUseDeviceName()
        {
            try
            {
                var status = VolumeHelper.CanUseDeviceName();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
