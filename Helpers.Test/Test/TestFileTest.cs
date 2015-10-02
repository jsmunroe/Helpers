using System;
using System.IO;
using Helpers.IO;
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
            var file = new TestFile(fileSystem, "\\file.dat");

            // Assert
            Assert.AreEqual("\\file.dat", file.Path);
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
            var file = new TestFile(a_fileSystem: null, a_path: "\\file.dat");

            // Assert
            Assert.AreEqual("\\file.dat", file.Path);
            Assert.IsNotNull(file.FileSystem);
        }


        [TestMethod]
        public void CallExistsOnExistingFile()
        {
            // Setup
            var path = "\\file\\does\\exist.dat";
            var fileSystem = new TestFileSystem();
            fileSystem.CreateFile(path, new TestFileStats());
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
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");
            var stream = new MemoryStream(new byte[1234]);

            // Execute
            file.Create(stream); // Stream is ignored by this implementation.

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual(1234, file.Size);
            Assert.AreEqual(DateTime.UtcNow.Date, file.CreatedTimeUtc.Date);
            Assert.AreEqual(DateTime.UtcNow.Hour, file.CreatedTimeUtc.Hour);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNullStream() // TestFile don't care.
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            file.Create(a_contents: null);
        }
        
        [TestMethod]
        public void DeleteFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");
            var stream = new MemoryStream();
            file.Create(stream);

            // Execute
            file.Delete();

            // Assert
            Assert.IsFalse(file.Exists);
        }


        [TestMethod]
        public void DeleteNotExistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            file.Delete();

            // Assert
            Assert.IsFalse(file.Exists);
        }

        [TestMethod]
        public void ChangeExtension()
        {
            // Setup
            var file = new TestFile(@"x:\directory\File.txt");

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
            var file = new TestFile(@"x:\directory\File.txt");

            // Execute
            var result = file.ChangeExtension(a_extension: null);
        }


        [TestMethod]
        public void CopyTo()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = fileSystem.CreateFile(@"x:\directory\File.bmp", new TestFileStats { Size = 240 });
            var dest = new TestFile(fileSystem, @"x:\directory\file2.bmp");

            // Execute
            file.CopyTo(dest);

            // Assert
            Assert.IsTrue(dest.Exists);
            Assert.AreEqual(240, dest.Size);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToWithNullDest()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = fileSystem.CreateFile(@"x:\directory\File.bmp", new TestFileStats { Size = 240 });

            // Execute
            file.CopyTo(a_dest: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyToWithNotExistingSource()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.CreateDirectory(@"x:\directory");
            var file = new TestFile(fileSystem, @"x:\directory\File.bmp");
            var dest = new TestFile(fileSystem, @"x:\directory\file2.bmp");

            // Execute
            file.CopyTo(dest);
        }

        [TestMethod]
        public void CopyToWithExistingDest()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = fileSystem.CreateFile(@"x:\directory\File.bmp", new TestFileStats { Size = 240 });
            var dest = fileSystem.CreateFile(@"x:\directory\File2.bmp", new TestFileStats { Size = 480 });

            // Execute
            file.CopyTo(dest);

            // Assert
            Assert.IsTrue(dest.Exists);
            Assert.AreEqual(240, dest.Size);
        }

        //[TestMethod]
        public void TestMethod()
        {
            var file = new FileInfo(@"C:\dir\NoExisty.txt");

            file.CopyTo(null);
        }


    }
}
