﻿using System;
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
            fileSystem.StageDirectory(path);
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
            fileSystem.StageDirectory(@"x:\mydirectory\directory1");
            fileSystem.StageDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.StageFile(@"x:\mydirectory\directory3\file.rgb", new TestFileStats());
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
            fileSystem.StageFile(@"x:\mydirectory\file1.dat", new TestFileStats());
            fileSystem.StageFile(@"x:\mydirectory\file2.dat", new TestFileStats());
            fileSystem.StageFile(@"x:\mydirectory\file3.dat", new TestFileStats());
            fileSystem.StageFile(@"x:\mydirectory\otherdirectory\file4.dat", new TestFileStats());
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
            fileSystem.StageDirectory(@"x:\mydirectory\directory1");
            fileSystem.StageDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.StageFile(@"x:\mydirectory\directory3\file.rgb", new TestFileStats());
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
            fileSystem.StageDirectory(@"x:\mydirectory\directory1");
            fileSystem.StageDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.StageFile(@"x:\mydirectory\directory3\file.rgb", new TestFileStats());
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
            var created = DateTime.UtcNow;
            var lastModified = DateTime.UtcNow;
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\mydirectory\file1.dat", new TestFileStats { Size = 1024, CreatedTimeUtc = created, LastModifiedTimeUtc = lastModified });
            fileSystem.StageFile(@"x:\mydirectory\file2.dat", new TestFileStats { Size = 14067, CreatedTimeUtc = created, LastModifiedTimeUtc = lastModified });
            fileSystem.StageFile(@"x:\mydirectory\file3.dat", new TestFileStats { Size = 2017, CreatedTimeUtc = created, LastModifiedTimeUtc = lastModified });
            fileSystem.StageFile(@"x:\mydirectory\otherdirectory\file4.dat", new TestFileStats { Size = 8740, CreatedTimeUtc = created, LastModifiedTimeUtc = lastModified });
            var directory = new TestDirectory(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.File("File1.dat");

            // Assert
            Assert.AreEqual("File1.dat", result.Name);
            Assert.IsTrue(result.Exists);
            Assert.AreEqual(1024, result.Size);
            Assert.AreEqual(created, result.LastModifiedTimeUtc);
            Assert.AreEqual(lastModified, result.LastModifiedTimeUtc);
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
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DeleteNotExistingDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = new TestDirectory(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            directory.Delete();
        }


        [TestMethod]
        public void EmptyDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var directory = fileSystem.StageDirectory(@"\directory");
            fileSystem.StageDirectory(@"\directory\child1");
            fileSystem.StageDirectory(@"\directory\child2");
            fileSystem.StageDirectory(@"\directory\child3");
            fileSystem.StageFile(@"\directory\file1.dat");
            fileSystem.StageFile(@"\directory\file2.dat");
            fileSystem.StageFile(@"\directory\file3.dat");
            fileSystem.StageFile(@"\directory\child2\fileA.dat");
            fileSystem.StageFile(@"\directory\child2\fileB.dat");
            fileSystem.StageFile(@"\directory\child2\fileC.dat");

            // Execute
            directory.Empty();

            // Assert
            Assert.IsTrue(fileSystem.DirectoryExists(@"\directory"));
            Assert.IsFalse(fileSystem.DirectoryExists(@"\directory\child1"));
            Assert.IsFalse(fileSystem.DirectoryExists(@"\directory\child2"));
            Assert.IsFalse(fileSystem.DirectoryExists(@"\directory\child3"));
            Assert.IsFalse(fileSystem.FileExists(@"\directory\file1.dat"));
            Assert.IsFalse(fileSystem.FileExists(@"\directory\file2.dat"));
            Assert.IsFalse(fileSystem.FileExists(@"\directory\file3.dat"));
            Assert.IsFalse(fileSystem.FileExists(@"\directory\child2\fileA.dat"));
            Assert.IsFalse(fileSystem.FileExists(@"\directory\child2\fileB.dat"));
            Assert.IsFalse(fileSystem.FileExists(@"\directory\child2\fileC.dat"));
        }


        [TestMethod]
        public void CustomTest_I()
        {
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"\Root\Directory");
            fileSystem.StageDirectory(@"\Root\Directory\Sub1");
            fileSystem.StageDirectory(@"\Root\Directory\Sub2");
            fileSystem.StageDirectory(@"\Root\Directory\Sub3");
            fileSystem.StageFile(@"\Root\Directory\File1.hsf", new TestFileStats());
            fileSystem.StageFile(@"\Root\Directory\File2.hsf", new TestFileStats());
            fileSystem.StageFile(@"\Root\Directory\File3.hsf", new TestFileStats());

            var directory = new TestDirectory(fileSystem, @"\");

            var result = directory.Directory(@"Root\Directory\Sub1").Exists;

            Assert.IsTrue(result);
        }


    }
}
