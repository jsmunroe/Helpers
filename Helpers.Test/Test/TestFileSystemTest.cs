using System;
using System.Collections.Generic;
using System.IO;
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
            var directoryPaths = fileSystem.GetDirectories(@"X:\");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }


        [TestMethod]
        public void DeleteDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\directory1");
            fileSystem.CreateDirectory(@"X:\DIRECTORY2\CHILD");
            fileSystem.CreateFile(@"x:\directory2\file.rgb");

            // Execute
            fileSystem.DeleteDirectory(@"x:\directory2");

            // Assert
            Assert.IsTrue(fileSystem.DirectoryExists(@"x:\directory1"));
            Assert.IsFalse(fileSystem.DirectoryExists((@"x:\directory2")));
            Assert.IsFalse(fileSystem.DirectoryExists(@"x:\directory2\child"));
            Assert.IsFalse(fileSystem.FileExists(@"x:\directory2\file.rgb"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteDirectoryWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.DeleteDirectory(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteDirectoryWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.DeleteDirectory(a_path: "?");
        }


        [TestMethod]
        public void DeleteDirectoryWithMissingDriveLetter()
        {
            // Setup
            var root = Path.GetPathRoot(Environment.SystemDirectory);
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory($@"{root}directory1");
            fileSystem.CreateDirectory($@"{root}directory2\child");
            fileSystem.CreateFile($@"{root}directory2\file.rgb");

            // Execute
            fileSystem.DeleteDirectory(@"\DIRECTORY2");

            // Assert
            Assert.IsTrue(fileSystem.DirectoryExists($@"{root}directory1"));
            Assert.IsFalse(fileSystem.DirectoryExists(($@"{root}directory2")));
            Assert.IsFalse(fileSystem.DirectoryExists($@"{root}directory2\child"));
            Assert.IsFalse(fileSystem.FileExists($@"{root}directory2\file.rgb"));
        }



        [TestMethod]
        public void DeleteNotExistingDirectory()
        {
            // Setup
            var root = Path.GetPathRoot(Environment.SystemDirectory);
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory($@"{root}directory1");
            fileSystem.CreateDirectory($@"{root}directory2\child");
            fileSystem.CreateFile($@"{root}directory2\file.rgb");

            // Execute
            fileSystem.DeleteDirectory(@"\directory3");

            // Assert
            Assert.IsTrue(fileSystem.DirectoryExists($@"{root}directory1"));
            Assert.IsTrue(fileSystem.DirectoryExists(($@"{root}directory2")));
            Assert.IsTrue(fileSystem.DirectoryExists($@"{root}directory2\child"));
            Assert.IsTrue(fileSystem.FileExists($@"{root}directory2\file.rgb"));
        }


        [TestMethod]
        public void DeleteFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(@"x:\directory2\file1.rgb");
            fileSystem.CreateFile(@"x:\directory2\file2.rgb");

            // Execute
            fileSystem.DeleteFile(@"x:\directory2\file2.rgb");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file1.rgb"));
            Assert.IsFalse(fileSystem.FileExists(@"x:\directory2\file2.rgb"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteFileWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.DeleteFile(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteFileWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.DeleteFile(a_path: "/i/am/on/linux");
        }


        [TestMethod]
        public void DeleteFileWithMissingDrive()
        {
            // Setup
            var root = Path.GetPathRoot(Environment.SystemDirectory);
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile($@"{root}directory2\file1.rgb");
            fileSystem.CreateFile($@"{root}directory2\file2.rgb");

            // Execute
            fileSystem.DeleteFile(@"\directory2\file2.rgb");

            // Assert
            Assert.IsTrue(fileSystem.FileExists($@"{root}directory2\file1.rgb"));
            Assert.IsFalse(fileSystem.FileExists($@"{root}directory2\file2.rgb"));
        }



        [TestMethod]
        public void DeleteNotExistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(@"x:\directory2\file1.rgb");
            fileSystem.CreateFile(@"x:\directory2\file2.rgb");

            // Execute
            fileSystem.DeleteFile(@"x:\directory2\file3.rgb");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file1.rgb"));
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file2.rgb"));
        }


    }
}
