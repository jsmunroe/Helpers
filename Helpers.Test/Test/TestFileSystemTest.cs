﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            fileSystem.StageDirectory(path);

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
            fileSystem.StageDirectory(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDirectoryWithBadPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is bad a directory path?";

            // Execute
            fileSystem.StageDirectory(path);
        }


        [TestMethod]
        public void CreateDirectoryMoreThanOnce()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is bad a directory path";

            // Execute
            fileSystem.StageDirectory(path);
            fileSystem.StageDirectory(path);
        }

        [TestMethod]
        public void DirectoryExists()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var path = @"X:\This is a directory path\";
            fileSystem.StageDirectory(path);

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
            fileSystem.StageDirectory(@"X:\This is a directory path\");

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
            fileSystem.StageDirectory(@"X:\Parent\Child");

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
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.StageFile(@"X:\Directory\File.dat", new TestFileInstance { Size = 14067, CreatedTimeUtc = created, LastModifiedTimeUtc = lastModified });

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"X:\Directory\File.dat"));
            var stats = fileSystem.GetFileInstance(@"X:\Directory\File.dat");
            Assert.AreEqual(14067, stats.Size);
            Assert.AreEqual(created, stats.CreatedTimeUtc);
            Assert.AreEqual(lastModified, stats.LastModifiedTimeUtc);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNull()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.StageFile(a_path: null, a_file: new TestFileInstance());
        }


        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))] Now allows null!
        public void CreateFileWithNullStats()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.StageFile(a_path: @"X:\Directory\File.dat", a_file: null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFileWithNotRootedPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.StageFile("thisIsABadPath.txt", new TestFileInstance());
        }

        [TestMethod]
        public void CreateFileOnRoot()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.StageFile(@"C:\File.dat", new TestFileInstance());

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"C:\File.dat"));
        }


        [TestMethod]
        public void GetFileStats()
        {
            // Setup
            var created = DateTime.UtcNow;
            var lastModified = DateTime.UtcNow;
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"X:\Directory\File.dat", new TestFileInstance { Size = 14067, CreatedTimeUtc = created, LastModifiedTimeUtc = lastModified });

            // Execute
            var stats = fileSystem.GetFileInstance(@"X:\Directory\File.dat");

            // Assert
            Assert.AreEqual(14067, stats.Size);
            Assert.AreEqual(created, stats.CreatedTimeUtc);
            Assert.AreEqual(lastModified, stats.LastModifiedTimeUtc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileStatsWithNullpath()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"X:\Directory\File.dat", new TestFileInstance());

            // Execute
            var stats = fileSystem.GetFileInstance(a_path: null);
        }
        
        [TestMethod]
        public void FileExists()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"X:\Directory\File.dat", new TestFileInstance());

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
            fileSystem.StageFile(@"x:\mydirectory\file1.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\file2.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\file3.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\otherdirectory\file4.dat", new TestFileInstance());

            // Execute
            var filePaths = fileSystem.GetFiles(@"X:\MYDIRECTORY");

            // Assert
            Assert.AreEqual(3, filePaths.Length);
        }


        [TestMethod]
        public void GetFilesInDirectoryWithSearchPattern()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\mydirectory\file1.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\file2.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\file3.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\otherdirectory\file4.dat", new TestFileInstance());

            // Execute
            var filePaths = fileSystem.GetFiles(@"X:\MYDIRECTORY", "file*.dat");

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
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetFilesInNonExistantDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.GetFiles(@"X:\MYDIRECTORY");
        }


        [TestMethod]
        public void GetFilesWithASearchPattern()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\mydirectory\file1.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\file2.css", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\file3.dat", new TestFileInstance());
            fileSystem.StageFile(@"x:\mydirectory\otherdirectory\file4.dat", new TestFileInstance());

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
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.GetFiles(@"X:\MYDIRECTORY", a_searchPattern: null);
        }

        [TestMethod]
        public void GetDirectoriesInDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"x:\mydirectory\directory1");
            fileSystem.StageDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.StageFile(@"x:\mydirectory\directory3\file.rgb", new TestFileInstance());

            // Execute
            var directoryPaths = fileSystem.GetDirectories(@"X:\MYDIRECTORY");

            // Assert
            Assert.AreEqual(3, directoryPaths.Length);
        }

        [TestMethod]
        public void GetDirectoriesWithSearchPattern()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"x:\mydirectory\directory1");
            fileSystem.StageDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.StageFile(@"x:\mydirectory\directory3\file.rgb", new TestFileInstance());

            // Execute
            var directoryPaths = fileSystem.GetDirectories(@"X:\MYDIRECTORY", "*1");

            // Assert
            Assert.AreEqual(1, directoryPaths.Length);
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
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetDirectoriesInNonExistantDirectory()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.GetDirectories(@"X:\MYDIRECTORY");
        }

        [TestMethod]
        public void GetDirectoryInRoot()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"x:\directory1");
            fileSystem.StageDirectory(@"x:\directory2\child");
            fileSystem.StageFile(@"x:\directory3\file.rgb", new TestFileInstance());

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
            fileSystem.StageDirectory(@"x:\directory1");
            fileSystem.StageDirectory(@"X:\DIRECTORY2\CHILD");
            fileSystem.StageFile(@"x:\directory2\file.rgb", new TestFileInstance());

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
        public void DeleteNotExistingDirectory()
        {
            // Setup
            var root = Path.GetPathRoot(Environment.SystemDirectory);
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory($@"{root}directory1");
            fileSystem.StageDirectory($@"{root}directory2\child");
            fileSystem.StageFile($@"{root}directory2\file.rgb", new TestFileInstance());

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
            fileSystem.StageFile(@"x:\directory2\file1.rgb", new TestFileInstance());
            fileSystem.StageFile(@"x:\directory2\file2.rgb", new TestFileInstance());

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
        public void DeleteNotExistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\directory2\file1.rgb", new TestFileInstance());
            fileSystem.StageFile(@"x:\directory2\file2.rgb", new TestFileInstance());

            // Execute
            fileSystem.DeleteFile(@"x:\directory2\file3.rgb");

            // Assert
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file1.rgb"));
            Assert.IsTrue(fileSystem.FileExists(@"x:\directory2\file2.rgb"));
        }


        [TestMethod]
        public void ReadFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\directory2\file1.rgb", new TestFileInstance("This is my file's data.", Encoding.UTF8));

            // Execute
            using (var stream = fileSystem.OpenRead(@"x:\directory2\file1.rgb"))
            {
                
                // Assert
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.CanRead);
                Assert.AreEqual(0, stream.Position);
                var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                Assert.AreEqual("This is my file's data.", data);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFileWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.OpenRead(a_path: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ReadFileWithNotExistingPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            using (var stream = fileSystem.OpenRead(@"x:\directory2\file1.rgb"))
            {

                // Assert
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.CanRead);
                Assert.AreEqual(0, stream.Position);
                var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                Assert.AreEqual("This is my file's data.", data);
            }
        }

        [TestMethod]
        public void ReadFileWithPathWithoutData()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\directory2\file1.rgb", new TestFileInstance());

            // Execute
            using (var stream = fileSystem.OpenRead(@"x:\directory2\file1.rgb"))
            {

                // Assert
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.CanRead);
                Assert.AreEqual(0, stream.Position);
                Assert.AreEqual(0, stream.Length);
            }
        }


        [TestMethod]
        public void WriteFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageFile(@"x:\directory2\file1.rgb", new TestFileInstance());

            // Execute
            using (var stream = fileSystem.OpenWrite(@"x:\directory2\file1.rgb"))
            {
                // Assert
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.CanWrite);
                Assert.AreEqual(0, stream.Position);
                var writer = new StreamWriter(stream);
                writer.Write("Yet more data.");
                writer.Flush();

            }

            using (var stream = fileSystem.OpenRead(@"x:\directory2\file1.rgb"))
            {
                var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                Assert.AreEqual("Yet more data.", data);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteFileWithNullPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            fileSystem.OpenWrite(a_path: null);
        }


        [TestMethod]
        public void WriteFileWithNotExistingPath()
        {
            // Setup
            var fileSystem = new TestFileSystem();

            // Execute
            using (var stream = fileSystem.OpenWrite(@"x:\directory2\file1.rgb"))
            {
                // Assert
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.CanWrite);
                Assert.AreEqual(0, stream.Position);
                var writer = new StreamWriter(stream);
                writer.Write("Yet more data.");
                writer.Flush();

            }

            using (var stream = fileSystem.OpenRead(@"x:\directory2\file1.rgb"))
            {
                var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                Assert.AreEqual("Yet more data.", data);
            }
        }


        [TestMethod]
        public void DeleteDirectoryWithPartialName()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"x:\directory\this is a directory");
            fileSystem.StageDirectory(@"x:\directory\this is");

            // Execute
            fileSystem.DeleteDirectory(@"x:\directory\this is");

            // Assert
            Assert.IsTrue(fileSystem.DirectoryExists(@"x:\directory\this is a directory"));
        }


    }
}
