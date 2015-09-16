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


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructTestDirectoryWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            new TestDirectory(a_fileSystem: fileSystem, a_path: null);
        }


        [TestMethod]
        public void ConstructTestDirectoryWithNullFileSystem()
        {
            // Execute
            var directory = new TestDirectory(a_fileSystem: null, a_path: "\\");

            // Assert
            Assert.AreEqual("\\", directory.Path);
            Assert.IsNotNull(directory.FileSystem);
        }


        [TestMethod]
        public void CallExistsOnExistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\directory\\does\\not\\exist");

            // Execute
            var result = directory.Exists;

            // Assert
            Assert.IsFalse(result);
        }



        [TestMethod]
        public void CallExistsWithNonexistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\directory\\does\\not\\exist");

            // Execute
            var result = directory.Exists;

            // Assert
            Assert.IsFalse(result);
        }






    }
}
