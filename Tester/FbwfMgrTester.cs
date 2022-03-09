using Fbwf.Library.Method;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Tester
{
    public class FbwfMgrTester
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
    }
}