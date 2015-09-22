using System;
using Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.IO
{
    [TestClass]
    public class FsFileTest
    {
        [TestMethod]
        public void ConstructFsFile()
        {
            // Execute
            var file = new FsFile(@"x:\directory\File.txt");

            // Assert
            Assert.AreEqual("File.txt", file.Name);
            Assert.AreEqual(@"x:\directory\File.txt", file.Path);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructFsFileWithNullPath()
        {
            // Execute
            var file = new FsFile(a_filePath: null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructFsFileWithBadPath()
        {
            // Execute
            var file = new FsFile("???");
        }

        [TestMethod]
        public void GetDirectory()
        {
            // Setup
            var file = new FsFile(@"x:\directory\File.txt");

            // Execute
            var result = file.Directory;

            // Assert
            Assert.AreEqual("directory", result.Name);
            Assert.AreEqual(@"x:\directory", result.Path);
        }

        [TestMethod]
        public void ChangeExtension()
        {
            // Setup
            var file = new FsFile(@"x:\directory\File.txt");

            // Execute
            var result = file.ChangeExtension("jpg");

            Assert.AreEqual("File.jpg", result.Name);
            Assert.AreEqual(@"x:\directory\File.jpg", result.Path);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangeExtensionWithNull()
        {
            // Setup
            var file = new FsFile(@"x:\directory\File.txt");

            // Execute
            var result = file.ChangeExtension(a_extension: null);
        }
    }
}
