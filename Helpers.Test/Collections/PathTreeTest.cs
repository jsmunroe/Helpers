using System;
using System.IO;
using Helpers.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Collections
{
    [TestClass]
    public class PathTreeTest
    {
        [TestMethod]
        public void ConstructPathTree()
        {
            // Execute
            new PathTree<string>();
        }

        [TestMethod]
        public void CreateDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
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
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.CreateDirectory(a_path: null);
        }

        [TestMethod]
        public void CreateDirectoryMoreThanOnce()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var path = @"X:\This is bad a directory path";

            // Execute
            fileSystem.CreateDirectory(path);
            fileSystem.CreateDirectory(path);
        }

        [TestMethod]
        public void DirectoryExists()
        {
            // Setup
            var fileSystem = new PathTree<string>();
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
            var fileSystem = new PathTree<string>();
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
            var fileSystem = new PathTree<string>();

            // Execute
            var results = fileSystem.DirectoryExists(a_path: null);
        }

        [TestMethod]
        public void DirectoryExistsWithMissingEndSlash()
        {
            // Setup
            var fileSystem = new PathTree<string>();
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
            var fileSystem = new PathTree<string>();
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
            var created = DateTime.UtcNow;
            var lastModified = DateTime.UtcNow;
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.CreateFile(@"X:\Directory\File.dat", "Value");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"X:\Directory\File.dat"));
            Assert.AreEqual("Value", fileSystem.GetLeafValue(@"X:\Directory\File.dat"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNull()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.CreateFile(a_path: null, a_value: "Value");
        }


        [TestMethod]
        public void CreateFileWithNullStats()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.CreateFile(a_path: @"X:\Directory\File.dat", a_value: null);
        }

        [TestMethod]
        public void CreateFileOnRoot()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.CreateFile(@"C:\File.dat", "Value");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"C:\File.dat"));
        }


        [TestMethod]
        public void CreateFileOnEmptyRoot()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.CreateFile(@"File.dat", "Value");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"File.dat"));
        }

        [TestMethod]
        public void GetLeafValue()
        {
            // Setup
            var created = DateTime.UtcNow;
            var lastModified = DateTime.UtcNow;
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"X:\Directory\File.dat", "Value");

            // Execute
            var result = fileSystem.GetLeafValue(@"X:\Directory\File.dat");

            // Assert
            Assert.AreEqual("Value", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileStatsWithNullpath()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"X:\Directory\File.dat", "Value");

            // Execute
            fileSystem.GetLeafValue(a_path: null);
        }

        [TestMethod]
        public void FileExists()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"X:\Directory\File.dat", "Value");

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
            var fileSystem = new PathTree<string>();

            // Execute
            var result = fileSystem.FileExists(a_path: null);
        }

        [TestMethod]
        public void GetFilesInDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat", "Value");

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
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.GetFiles(a_path: null);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetFilesInNonExistantDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.GetFiles(@"X:\MYDIRECTORY");
        }

        [TestMethod]
        public void GetFilesWithASearchPattern()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat", "Value1");
            fileSystem.CreateFile(@"x:\mydirectory\file2.css", "Value2");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat", "Value3");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat", "Value4");

            // Execute
            var filePaths = fileSystem.GetFiles(@"X:\MYDIRECTORY", "*.dat");

            // Assert
            Assert.AreEqual(2, filePaths.Length);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFilesWithNullSearchPattern()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.GetFiles(@"X:\MYDIRECTORY", a_searchPattern: null);
        }

        [TestMethod]
        public void GetDirectoriesInDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb", "Value");

            // Execute
            string[] directoryPaths = fileSystem.GetDirectories(@"X:\MYDIRECTORY");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }


        [TestMethod]
        public void GetDirectoriesInRoot()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory("directory1");
            fileSystem.CreateDirectory(@"directory2\child");
            fileSystem.CreateFile(@"directory3\file.rgb", "Value");
            fileSystem.CreateDirectory(@"x:\directory4");
            fileSystem.CreateDirectory(@"\directory5");

            // Execute
            string[] directoryPaths = fileSystem.GetDirectories("");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDirectoriesInDirectoryWithNull()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.GetDirectories(a_path: null);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetDirectoriesInNonExistantDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.GetDirectories(@"X:\MYDIRECTORY");
        }

        [TestMethod]
        public void GetDirectoryInRoot()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"x:\directory1");
            fileSystem.CreateDirectory(@"x:\directory2\child");
            fileSystem.CreateFile(@"x:\directory3\file.rgb", "Value");

            // Execute
            var directoryPaths = fileSystem.GetDirectories(@"X:\");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }


        [TestMethod]
        public void DeleteDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"x:\directory1");
            fileSystem.CreateDirectory(@"X:\DIRECTORY2\CHILD");
            fileSystem.CreateFile(@"x:\directory2\file.rgb", "Value");

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
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.DeleteDirectory(a_path: null);
        }

        [TestMethod]
        public void DeleteNotExistingDirectory()
        {
            // Setup
            var root = Path.GetPathRoot(Environment.SystemDirectory);
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory($@"{root}directory1");
            fileSystem.CreateDirectory($@"{root}directory2\child");
            fileSystem.CreateFile($@"{root}directory2\file.rgb", "Value");

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
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\directory2\file1.rgb", "Value");
            fileSystem.CreateFile(@"x:\directory2\file2.rgb", "Value");

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
            var fileSystem = new PathTree<string>();

            // Execute
            fileSystem.DeleteFile(a_path: null);
        }

        [TestMethod]
        public void DeleteNotExistingFile()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\directory2\file1.rgb", "Value");
            fileSystem.CreateFile(@"x:\directory2\file2.rgb", "Value");

            // Execute
            fileSystem.DeleteFile(@"x:\directory2\file3.rgb");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file1.rgb"));
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file2.rgb"));
        }


    }
}