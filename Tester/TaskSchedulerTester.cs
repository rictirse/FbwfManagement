using Fbwf.Library.Method;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fbwf.Tester
{
    internal class TaskSchedulerTester
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void MountScriptWriteToFile()
        {
            try
            {
                FbwfTaskScheduler.MountScriptWriteToFile();
                Assert.True(File.Exists(AssemblyData.MountScript));
                //File.Delete(AssemblyData.MountScript);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task MountScriptWriteToFileAsync()
        {
            try
            {
                await FbwfTaskScheduler.MountScriptWriteToFileAsync();
                Assert.True(File.Exists(AssemblyData.MountScript));
                //File.Delete(AssemblyData.MountScript);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void Exists()
        {
            try
            {
                Assert.IsFalse(FbwfTaskScheduler.Exists());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void WriteScheduler()
        {
            try
            {
                FbwfTaskScheduler.Write();
                Assert.IsTrue(FbwfTaskScheduler.Exists());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void DeleteTask()
        {
            try
            {
                FbwfTaskScheduler.Delete();
                Assert.False(FbwfTaskScheduler.Exists());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
