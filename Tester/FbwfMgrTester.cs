using Fbwf.Library.Method;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fbwf.Tester
{
    internal class FbwfMgrTester
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DisplayConfig()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task DisplayConfigAsync()
        {
            try
            {
                await FbwfMgr.DisplayConfigAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task RefreshAsync()
        {
            try
            {
                var fbwf = new FbwfMgr();
                await fbwf.RefreshAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task RemoveOtherVolumeAsync()
        {
            try
            {
                var diskLetter = "E";
                var fbwf = new FbwfMgr();
                await fbwf.RefreshAsync();
                var protectedVolume = fbwf.NextSession.ProtectedVolume;

                if (!protectedVolume.Any(x => x == $"{diskLetter}:"))
                {
                    Debug.WriteLine("Exists");
                }

                foreach (var _diskLetter in protectedVolume.Except(new[] { $"{diskLetter}:" }))
                {
                    Debug.WriteLine(_diskLetter);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void SetCacheSize()
        {
            try
            {
                var result = FbwfMgr.SetCacheSize(384);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task SetCacheSizeAsync()
        {
            try
            {
               var result = await FbwfMgr.SetCacheSizeAsync(384);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void AddVolume()
        {
            try
            {
                var result = FbwfMgr.AddVolume('E');
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void RemoveVolume()
        {
            try
            {
                var result = FbwfMgr.RemoveVolume('F');
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task AddVolumeAsync()
        {
            try
            {
                //var fi = new FileInfo(AssemblyData.VhdPath);
                //await VHDMounter.CreatVHDAsync(fi);
                //await VHDMounter.AttachAsync(fi, 'R');
                var result = await FbwfMgr.AddVolumeAsync('F');
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task RemoveVolumeAsync()
        {
            try
            {
                var result = await FbwfMgr.RemoveVolumeAsync('E');
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}