using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Test
{
    [TestClass]
    public class TestDirectoryTest
    {
        [TestMethod]
        public void ConstructTestDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            var directory = new TestDirectory(fileSystem, "\\");

            // Assert
            Assert.AreEqual("\\", directory.Path);
            Assert.AreSame(fileSystem, directory.FileSystem);
        }
    }
}
