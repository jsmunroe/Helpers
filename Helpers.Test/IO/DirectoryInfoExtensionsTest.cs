using System;
using System.IO;
using Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.IO
{
    [TestClass]
    public class DirectoryInfoExtensionsTest
    {
        [TestMethod]
        public void GetChildFile()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var file = directory.File("File");

            // Assert
            Assert.AreEqual(@"C:\Directory\File", file.FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildFileWithNullFileName()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var file = directory.File(a_fileName: null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetChildFileFromNullDirectory()
        {
            // Setup
            DirectoryInfo directory = null;

            // Execute
            var file = directory.File("File");
        }

        [TestMethod]
        public void GetChildFilePath()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var path = directory.FilePath("File");

            // Assert
            Assert.AreEqual(@"C:\Directory\File", path);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildFilePathWithNullFileName()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var path = directory.FilePath(a_fileName: null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetChildFilePathFromNullDirectory()
        {
            // Setup
            DirectoryInfo directory = null;

            // Execute
            var path = directory.FilePath("File");
        }

        [TestMethod]
        public void GetChildDirectory()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var child = directory.Directory("Directory");

            // Assert
            Assert.AreEqual(@"C:\Directory\Directory", child.FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildWithNullDirectoryName()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var child = directory.Directory(a_directoryName: null);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetChildDirectoryFromNullDirectory()
        {
            // Setup
            DirectoryInfo directory = null;

            // Execute
            var child = directory.Directory("Directory");
        }

        [TestMethod]
        public void GetChildDirectoryPath()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var path = directory.DirectoryPath("Directory");

            // Assert
            Assert.AreEqual(@"C:\Directory\Directory", path);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildDirectoryPathWithNull()
        {
            // Setup
            var directory = new DirectoryInfo(@"\Directory");

            // Execute
            var path = directory.DirectoryPath(a_directoryName: null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetChildDirectoryPathFromNullDirectory()
        {
            // Setup
            DirectoryInfo directory = null;

            // Execute
            var Path = directory.DirectoryPath("Directory");
        }


    }
}
