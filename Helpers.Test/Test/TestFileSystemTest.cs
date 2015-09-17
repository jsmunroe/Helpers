﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Test
{
    [TestClass]
    public class TestFileSystemTest
    {
        [TestMethod]
        public void ConstructTestFileSystemTest()
        {
            // Execute
            new TestFileSystem();
        }


        [TestMethod]
        public void CreateDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is a directory path\";

            // Execute
            fileSystem.CreateDirectory(path);

            // Assert
            Assert.IsTrue(fileSystem.DirectoryExists(path));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDirectoryWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.CreateDirectory(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDirectoryWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is bad a directory path?";

            // Execute
            fileSystem.CreateDirectory(path);
        }


        [TestMethod]
        public void CreateDirectoryMoreThanOnce()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is bad a directory path";

            // Execute
            fileSystem.CreateDirectory(path);
            fileSystem.CreateDirectory(path);
        }

        [TestMethod]
        public void DirectoryExists()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is a directory path\";
            fileSystem.CreateDirectory(path);

            // Execute
            path = path.ToLower(); // Ignores case.
            var results = fileSystem.DirectoryExists(path.ToLower()); 

            // Assert
            Assert.IsTrue(results);
        }


        [TestMethod]
        public void DirectoryExistsWithNotExistingDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is a directory path\";

            // Execute
            path = path.ToLower(); // Ignores case.
            var results = fileSystem.DirectoryExists(path.ToLower());

            // Assert
            Assert.IsFalse(results);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DirectoryExistsWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            var results = fileSystem.DirectoryExists(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DirectoryExistsWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is bad a directory path?";

            // Execute
            fileSystem.DirectoryExists(path);
        }

        [TestMethod]
        public void DirectoryExistsWithMissingEndSlash()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"X:\This is a directory path\");

            // Execute
            var results = fileSystem.DirectoryExists(@"X:\This is a directory path");

            // Assert
            Assert.IsTrue(results);
        }

        [TestMethod]
        public void DirectoryExistsOnChildDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"X:\Parent\Child");

            // Execute
            var results = fileSystem.DirectoryExists(@"X:\Parent");

            // Assert
            Assert.IsTrue(results);
        }

        [TestMethod]
        public void CreateFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.CreateFile(@"X:\Directory\File.dat");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"X:\Directory\File.dat"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.CreateFile(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFileWithNotRootedPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.CreateFile(a_path: "thisIsABadPath.txt");
        }

        [TestMethod]
        public void CreateFileOnRoot()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.CreateFile(@"C:\File.dat");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"C:\File.dat"));
        }


        [TestMethod]
        public void FileExists()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(@"X:\Directory\File.dat");

            // Execute
            var result = fileSystem.FileExists(@"x:\directory\file.DAT");

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileExistsWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            var result = fileSystem.FileExists(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FileExistsWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            var result = fileSystem.FileExists(a_path: @"X:\ccccc?.bat");
        }


        [TestMethod]
        public void GetFilesInDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat");

            // Execute
            var filePaths = fileSystem.GetFiles(@"X:\MYDIRECTORY");

            // Assert
            Assert.AreEqual(3, filePaths.Length);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFilesInDirectoryWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
           fileSystem.GetFiles(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFilesinDirectoryWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.GetFiles(a_path: "meow meow meow");
        }


        [TestMethod]
        public void GetFilesInNonExistantDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            var filePaths = fileSystem.GetFiles(@"X:\MYDIRECTORY");

            // Assert
            Assert.AreEqual(0, filePaths.Length);
        }

        [TestMethod]
        public void GetDirectoriesInDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb");

            // Execute
            string[] directoryPaths = fileSystem.GetDirectories(@"X:\MYDIRECTORY");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDirectoriesInDirectoryWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.GetDirectories(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDirectoriesInBadDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.GetDirectories(a_path: "x:\\????");
        }


        [TestMethod]
        public void GetDirectoryInRoot()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\directory1");
            fileSystem.CreateDirectory(@"x:\directory2\child");
            fileSystem.CreateFile(@"x:\directory3\file.rgb");

            // Execute
            string[] directoryPaths = fileSystem.GetDirectories(@"X:\");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }


        [TestMethod]
        public void DeleteDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteDirectoryWithNullPath()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteDirectoryWithBadPath()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void DeleteNotExistingDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void DeleteFile()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteFileWithNullPath()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteFileWithBadPath()
        {
            Assert.Inconclusive("Not implemented!");
        }
    }
}
