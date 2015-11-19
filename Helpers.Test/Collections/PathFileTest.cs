using System;
using System.IO;
using Helpers.Collections;
using Helpers.Contracts;
using Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Helpers.Test.Collections
{
    [TestClass]
    public class PathFileTest
    {
        [TestMethod]
        public void ConstructPathFile()
        {
            // Setup
            var pathTree = new PathTree<string>();

            // Execute
            var file = new PathFile<string>(pathTree, "\\file.dat");

            // Assert
            Assert.AreEqual("\\file.dat", file.Path);
            Assert.AreSame(pathTree, file.FileSystem);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructPathFileWithNullPath()
        {
            // Setup
            var pathTree = new PathTree<string>();

            // Execute
            new PathFile<string>(a_pathTree: pathTree, a_path: null);
        }


        [TestMethod]
        public void ConstructPathFileWithNullPathTree()
        {
            // Execute
            var file = new PathFile<string>(a_pathTree: null, a_path: "\\file.dat");

            // Assert
            Assert.AreEqual("\\file.dat", file.Path);
            Assert.IsNotNull(file.FileSystem);
        }


        [TestMethod]
        public void CallExistsWithExistingFile()
        {
            // Setup
            var path = "\\file\\does\\exist.dat";
            var pathTree = new PathTree<string>();
            pathTree.CreateFile(path, "Value");
            var file = new PathFile<string>(pathTree, path);

            // Execute
            var result = file.Exists;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CallExistsWithNotExistingFile()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\file\\does\\not\\exist.dat");

            // Execute
            var result = file.Exists;

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void GetName()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");

            // Execute
            var result = file.Name;

            // Assert
            Assert.AreEqual("file.txt", result);
        }

        [TestMethod]
        public void GetDirectory()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");

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
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");

            // Execute
            file.Create("Value");

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual("Value", file.Value);
        }


        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))] // Value is allowed
        public void CreateFileWithNullValue()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");

            // Execute
            file.Create(a_value: null);
        }

        [TestMethod]
        public void DeleteFile()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");
            file.Create("Value");

            // Execute
            file.Delete();

            // Assert
            Assert.IsFalse(file.Exists);
        }


        [TestMethod]
        public void DeleteNotExistingFile()
        {
            // Setup
            var pathTree = new PathTree<string>();
            pathTree.CreateDirectory("\\this\\is\\a");
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");

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
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, "\\this\\is\\a\\file.txt");

            // Execute
            file.Delete();
        }

        [TestMethod]
        public void ChangeExtension()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, @"x:\directory\File.txt");

            // Execute
            var result = file.ChangeExtension("jpg");

            Assert.AreEqual("File.jpg", result.Name);
            Assert.AreEqual(@"x:\directory\File.jpg", result.Path);
        }


        [TestMethod]
        public void ChangeExtensionOnNotRootedFile()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, @"File.txt");

            // Execute
            var result = file.ChangeExtension("jpg");

            Assert.AreEqual("File.jpg", result.Name);
            Assert.AreEqual("File.jpg", result.Path);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangeExtensionWithNull()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, @"x:\directory\File.txt");

            // Execute
            var result = file.ChangeExtension(a_extension: null);
        }


        [TestMethod]
        public void CopyTo()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = pathTree.CreateFile(@"x:\directory\File.bmp", "Value");
            var dest = new PathFile<string>(pathTree, @"x:\directory\file2.bmp");

            // Execute
            file.CopyTo(dest);

            // Assert
            Assert.IsTrue(dest.Exists);
            Assert.AreEqual("Value", dest.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToWithNullDest()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = pathTree.CreateFile(@"x:\directory\File.bmp", "Value");

            // Execute
            file.CopyTo(a_dest: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyToWithNotExistingSource()
        {
            // Setup
            var pathTree = new PathTree<string>();
            pathTree.CreateDirectory(@"x:\directory");
            var file = new PathFile<string>(pathTree, @"x:\directory\File.bmp");
            var dest = new PathFile<string>(pathTree, @"x:\directory\file2.bmp");

            // Execute
            file.CopyTo(dest);
        }

        [TestMethod]
        public void CopyToWithExistingDest()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = pathTree.CreateFile(@"x:\directory\File.bmp", "Value");
            var dest = pathTree.CreateFile(@"x:\directory\File2.bmp", "Value2");

            // Execute
            file.CopyTo(dest);

            // Assert
            Assert.IsTrue(dest.Exists);
            Assert.AreEqual("Value", dest.Value);
        }

        [TestMethod]
        public void CopyFrom()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var source = pathTree.CreateFile(@"x:\directory\File.bmp", "Value");
            var file = new PathFile<string>(pathTree, @"x:\directory\file2.bmp");

            // Execute
            file.CopyFrom(source);

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual("Value", file.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyFromWithNullDest()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var file = new PathFile<string>(pathTree, @"x:\directory\file2.bmp");

            // Execute
            file.CopyFrom(a_source: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyFromWithNotExistingSource()
        {
            // Setup
            var pathTree = new PathTree<string>();
            pathTree.CreateDirectory(@"x:\directory");
            var source = new PathFile<string>(pathTree, @"x:\directory\File.bmp");
            var file = new PathFile<string>(pathTree, @"x:\directory\file2.bmp");

            // Execute
            file.CopyFrom(source);
        }

        [TestMethod]
        public void CopyFromWithExistingDest()
        {
            // Setup
            var pathTree = new PathTree<string>();
            var source = pathTree.CreateFile(@"x:\directory\File.bmp", "Value");
            var file = pathTree.CreateFile(@"x:\directory\File2.bmp", "Value2");

            // Execute
            file.CopyFrom(source);

            // Assert
            Assert.IsTrue(file.Exists);
            Assert.AreEqual("Value", file.Value);
        }

        [TestMethod]
        public void CopyToWithCopier()
        {
            // Setup
            var mockCopier = new Mock<IFileCopier<PathFile<string>, TestFile>>();
            var pathTree = new PathTree<string>();
            var file = pathTree.CreateFile(@"x:\directory\File.dat", "Value");
            var dest = new TestFile(@"C:\temp\test.dat");

            // Execute
            file.CopyTo(dest, mockCopier.Object);

            // Assert
            mockCopier.Verify(i => i.Copy(file, dest), Times.Once);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToWithCopierWithNullDestinationFile()
        {
            // Setup
            var mockCopier = new Mock<IFileCopier<PathFile<string>, TestFile>>();
            var pathTree = new PathTree<string>();
            var file = pathTree.CreateFile(@"x:\directory\File.dat", "Value");
            var dest = new TestFile(@"C:\temp\test.dat");

            // Execute
            file.CopyTo(a_dest: null, a_fileCopier: mockCopier.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToWithNullCopier()
        {
            // Setup
            var mockCopier = new Mock<IFileCopier<PathFile<string>, TestFile>>();
            var pathTree = new PathTree<string>();
            var file = pathTree.CreateFile(@"x:\directory\File.dat", "Value");
            var dest = new TestFile(@"C:\temp\test.dat");

            // Execute
            file.CopyTo(a_dest: dest, a_fileCopier: null);
        }


        //[TestMethod]
        public void Temp()
        {
            var fileSystem = new TestFileSystem();
            var pathTree = new PathTree<IFile>();
            pathTree.CreateDirectory(@"urban stuff");
            pathTree.CreateFile(@"urban stuff\urban1.hsf", fileSystem.StageFile(@"x:\root\urban stuff\urban1.hsf"));
            pathTree.CreateFile(@"urban stuff\urban1.jpg", fileSystem.StageFile(@"x:\root\urban stuff\urban1.jpg"));
            pathTree.CreateFile(@"urban stuff\urban2.hsf", fileSystem.StageFile(@"x:\root\urban stuff\urban2.hsf"));
            pathTree.CreateFile(@"urban stuff\urban2.jpg", fileSystem.StageFile(@"x:\root\urban stuff\urban2.jpg"));
            pathTree.CreateFile(@"urban stuff\urban3.hsf", fileSystem.StageFile(@"x:\root\urban stuff\urban3.hsf"));
            pathTree.CreateFile(@"urban stuff\urban3.jpg", fileSystem.StageFile(@"x:\root\urban stuff\urban3.jpg"));
            pathTree.CreateDirectory(@"curved case stuff");
            pathTree.CreateFile(@"curved case stuff\curved1.hsf", fileSystem.StageFile(@"x:\root\curved case stuff\curved1.hsf"));
            pathTree.CreateFile(@"curved case stuff\curved1.jpg", fileSystem.StageFile(@"x:\root\curved case stuff\curved1.jpg"));
            pathTree.CreateFile(@"curved case stuff\curved2.hsf", fileSystem.StageFile(@"x:\root\curved case stuff\curved2.hsf"));
            pathTree.CreateFile(@"curved case stuff\curved2.jpg", fileSystem.StageFile(@"x:\root\curved case stuff\curved2.jpg"));
            pathTree.CreateFile(@"curved case stuff\curved3.hsf", fileSystem.StageFile(@"x:\root\curved case stuff\curved3.hsf"));
            pathTree.CreateFile(@"curved case stuff\curved3.jpg", fileSystem.StageFile(@"x:\root\curved case stuff\curved3.jpg"));

            IFile file = new PathFile<IFile>(pathTree, @"urban stuff\urban1.hsf");
            IFile destination = new PathFile<IFile>(pathTree, @"curved case stuff\carved urban1.esa");

            if (file.Exists)
                file.CopyTo(destination);

            file = file.ChangeExtension(".jpg");
            destination = destination.ChangeExtension(".jpg");

            if (file.Exists)
                file.CopyTo(destination);

            Assert.IsFalse(pathTree.FileExists(@"urban stuff\urban1.hsf"));
            Assert.IsTrue(pathTree.FileExists(@"curved case stuff\carved urban1.esa"));
        }


    }
}