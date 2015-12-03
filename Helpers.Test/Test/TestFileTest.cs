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
            fileSystem.StageFile(path, new TestFileInstance());
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
            file.Create(stream);

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual(1234, file.Size);
            Assert.AreEqual(DateTime.UtcNow.Date, file.CreatedTimeUtc.Date);
            Assert.AreEqual(DateTime.UtcNow.Hour, file.CreatedTimeUtc.Hour);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNullStream()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            file.Create(a_contents: null);
        }


        [TestMethod]
        public void CreateFileWithText()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, @"\a\file\this\is.txt");

            // Execute
            file.Create("This is a file!"); 

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual(15, file.Size); // File length determined here: https://mothereff.in/byte-counter#This%20is%20a%20file%21
            Assert.AreEqual(DateTime.UtcNow.Date, file.CreatedTimeUtc.Date);
            Assert.AreEqual(DateTime.UtcNow.Hour, file.CreatedTimeUtc.Hour);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFileWithNullText()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, @"\a\file\this\is.txt");

            // Execute
            file.Create(a_text: null);
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
            fileSystem.StageDirectory("\\this\\is\\a");
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            file.Delete();

            // Assert
            Assert.IsFalse(file.Exists);
        }


        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DeleteFromNotExistingFile()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, "\\this\\is\\a\\file.txt");

            // Execute
            file.Delete();
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
            var file = fileSystem.StageFile(@"x:\directory\File.bmp", new TestFileInstance { Size = 240 });
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
            var file = fileSystem.StageFile(@"x:\directory\File.bmp", new TestFileInstance { Size = 240 });

            // Execute
            file.CopyTo(a_dest: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyToWithNotExistingSource()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"x:\directory");
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
            var file = fileSystem.StageFile(@"x:\directory\File.bmp", new TestFileInstance { Size = 240 });
            var dest = fileSystem.StageFile(@"x:\directory\File2.bmp", new TestFileInstance { Size = 480 });

            // Execute
            file.CopyTo(dest);

            // Assert
            Assert.IsTrue(dest.Exists);
            Assert.AreEqual(240, dest.Size);
        }
        [TestMethod]
        public void CopyFrom()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var source = fileSystem.StageFile(@"x:\directory\File.bmp", new TestFileInstance { Size = 240 });
            var file = new TestFile(fileSystem, @"x:\directory\file2.bmp");

            // Execute
            file.CopyFrom(source);

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual(240, file.Size);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyFromWithNullDest()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = new TestFile(fileSystem, @"x:\directory\file2.bmp");

            // Execute
            file.CopyFrom(a_source: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyFromWithNotExistingSource()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            fileSystem.StageDirectory(@"x:\directory");
            var source = new TestFile(fileSystem, @"x:\directory\File.bmp");
            var file = new TestFile(fileSystem, @"x:\directory\file2.bmp");

            // Execute
            file.CopyFrom(source);
        }

        [TestMethod]
        public void CopyFromWithExistingDest()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var source = fileSystem.StageFile(@"x:\directory\File.bmp", new TestFileInstance { Size = 240 });
            var file = fileSystem.StageFile(@"x:\directory\File2.bmp", new TestFileInstance { Size = 480 });

            // Execute
            file.CopyFrom(source);

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual(240, file.Size);
        }


        [TestMethod]
        public void OpenWrite()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = fileSystem.StageFile(@"x:\directory\File.xml", new TestFileInstance ());

            // Execute
            using (var stream = file.OpenWrite())
            {
                var writer = new StreamWriter(stream);
                writer.Write("Some data.");
                writer.Flush();

                // Assert
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.Length > 0);
            }

            // Assert
            using (var stream = file.OpenRead())
            {
                var reader = new StreamReader(stream);
                Assert.AreEqual("Some data.", reader.ReadToEnd());
            }
        }


        [TestMethod]
        public void OpenWriteWithNotExistingFile()
        {
            // Setup
            var file = new TestFile(@"x:\directory\File.xml");

            // Execute
            using (var stream = file.OpenWrite())
            {
                var writer = new StreamWriter(stream);
                writer.Write("Some data.");
                writer.Flush();

                // Assert
                Assert.IsTrue(file.Exists); // File was created on write.
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.Length > 0);
            }

            // Assert
            using (var stream = file.OpenRead())
            {
                var reader = new StreamReader(stream);
                Assert.AreEqual("Some data.", reader.ReadToEnd());
            }
        }



        [TestMethod]
        public void OpenRead()
        {
            // Setup
            var fileSystem = new TestFileSystem();
            var file = fileSystem.StageFile(@"x:\directory\File.xml", new TestFileInstance("This is data."));

            // Assert
            using (var stream = file.OpenRead())
            {
                var reader = new StreamReader(stream);
                Assert.AreEqual("This is data.", reader.ReadToEnd());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void OpenReadWithNotExistingFile()
        {
            // Setup
            var file = new TestFile(@"x:\directory\File.xml");

            // Execute
            file.OpenRead();
        }




        // [CustomTest_I]
        public void TestMethod()
        {
            var file = new FileInfo(@"C:\NoExisty.txt");

            file.Delete();
        }


    }
}
