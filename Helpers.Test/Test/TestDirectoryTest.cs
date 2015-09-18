using System;
using System.IO;
using System.Linq;
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
            var path = "\\directory\\does\\exist";
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(path);
            var directory = new TestDirectory(fileSystem, path);

            // Execute
            var result = directory.Exists;

            // Assert
            Assert.IsTrue(result);
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


        [TestMethod]
        public void GetName()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            var result = directory.Name;

            // Assert
            Assert.AreEqual("directory", result);
        }


        [TestMethod]
        public void GetParent()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            var result = directory.Parent;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Name);
        }


        [TestMethod]
        public void GetChildDirectories()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb");
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.Directories;

            // Assert
            Assert.AreEqual(3, result.Count());
        }


        [TestMethod]
        public void GetChildFiles()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat");
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var filePaths = directory.Files;

            // Assert
            Assert.AreEqual(3, filePaths.Count());
        }

        [TestMethod]
        public void GetChildDirectoryByName()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb");
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.Directory("Directory2");

            // Assert
            Assert.AreEqual("Directory2", result.Name);
            Assert.IsTrue(result.Exists);
        }


        [TestMethod]
        public void GetNotExistingChildDirectoryByName()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb");
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.Directory("Directory4");

            // Assert
            Assert.AreEqual("Directory4", result.Name);
            Assert.IsFalse(result.Exists);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildDirectoryByNameWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.Directory(a_name: null);
        }

        [TestMethod]
        public void GetChildFileByName()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat");
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.File("File1.dat");

            // Assert
            Assert.AreEqual("File1.dat", result.Name);
            Assert.IsTrue(result.Exists);
        }

        [TestMethod]
        public void CreateDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            directory.Create();

            // Assert
            Assert.IsTrue(directory.Exists);
        }

        [TestMethod]
        public void DeleteDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\this\\is\\a\\directory");
            directory.Create();

            // Execute
            directory.Delete();

            // Assert
            Assert.IsFalse(directory.Exists);
        }


        [TestMethod]
        public void DeleteNotExistingDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            directory.Delete();

            // Assert
            Assert.IsFalse(directory.Exists);
        }


    }
}
