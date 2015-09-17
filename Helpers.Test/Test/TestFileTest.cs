using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Test
{
    [TestClass]
    public class TestFileTest
    {
        [TestMethod]
        public void ConstructTestFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            var file = new TestFile(fileSystem, "\\");

            // Assert
            Assert.AreEqual("\\", file.Path);
            Assert.AreSame(fileSystem, file.FileSystem);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructTestFileWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            new TestFile(a_fileSystem: fileSystem, a_path: null);
        }


        [TestMethod]
        public void ConstructTestFileWithNullFileSystem()
        {
            // Execute
            var file = new TestFile(a_fileSystem: null, a_path: "\\");

            // Assert
            Assert.AreEqual("\\", file.Path);
            Assert.IsNotNull(file.FileSystem);
        }


        [TestMethod]
        public void CallExistsOnExistingFile()
        {
            // Setup
            var path = "\\file\\does\\exist.dat";
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(path);
            var file = new TestFile(fileSystem, path);

            // Execute
            var result = file.Exists;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CallExistsWithNonexistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\file\\does\\not\\exist.dat");

            // Execute
            var result = file.Exists;

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void GetName()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            var result = file.Name;

            // Assert
            Assert.AreEqual("file.txt", result);
        }

        [TestMethod]
        public void GetDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            var result = file.Directory;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Name);
        }


        [TestMethod]
        public void CreateFile()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNullStream()
        {
            Assert.Inconclusive("Not implemented!");
        }
        
        [TestMethod]
        public void DeleteFile()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void DeleteNotExistingFile()
        {
            Assert.Inconclusive("Not implemented!");
        }
    }
}
