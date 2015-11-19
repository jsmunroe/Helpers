using System;
using System.IO;
using System.Linq;
using Helpers.Collections;
using Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Collections
{
    [TestClass]
    public class PathDirectoryTest
    {
        [TestMethod]
        public void ConstructPathDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            var directory = new PathDirectory<string>(fileSystem, "\\");

            // Assert
            Assert.AreEqual("\\", directory.Path);
            Assert.AreSame(fileSystem, directory.FileSystem);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructPathDirectoryWithNullPath()
        {
            // Setup
            var fileSystem = new PathTree<string>();

            // Execute
            new PathDirectory<string>(a_pathTree: fileSystem, a_path: null);
        }


        [TestMethod]
        public void ConstructPathDirectoryWithNullFileSystem()
        {
            // Execute
            var directory = new PathDirectory<string>(a_pathTree: null, a_path: "\\");

            // Assert
            Assert.AreEqual("\\", directory.Path);
            Assert.IsNotNull(directory.FileSystem);
        }


        [TestMethod]
        public void GetExistsOnExistingFile()
        {
            // Setup
            var path = "\\directory\\does\\exist";
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(path);
            var directory = new PathDirectory<string>(fileSystem, path);

            // Execute
            var result = directory.Exists;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetExistsWithNonexistingFile()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\directory\\does\\not\\exist");

            // Execute
            var result = directory.Exists;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetIsEmptyOnEmptyDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory("\\directory\\does\\not\\exist");

            // Execute
            var result = directory.IsEmpty;

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void GetIsEmptyOnNonemptyDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory("\\directory\\does\\not\\exist");

            // Execute
            var result = directory.Parent.IsEmpty;

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void GetIsEmptyOnNotExistingDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\directory\\does\\not\\exist");

            // Execute
            var result = directory.IsEmpty;

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void GetName()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            var result = directory.Name;

            // Assert
            Assert.AreEqual("directory", result);
        }


        [TestMethod]
        public void GetParent()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\this\\is\\a\\directory");

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
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb", "Value");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.Directories();

            // Assert
            Assert.AreEqual(3, result.Count());
        }


        [TestMethod]
        public void GetChildFiles()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat", "Value");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var filePaths = directory.Files();

            // Assert
            Assert.AreEqual(3, filePaths.Count());
        }

        [TestMethod]
        public void GetChildFilesBySearchPattern()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\file3.css", "Value");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat", "Value");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var filePaths = directory.Files("*.css");

            // Assert
            Assert.AreEqual(1, filePaths.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChildFilesWithNullSearchPattern()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            directory.Files(a_pattern: null);
        }



        [TestMethod]
        public void GetChildDirectoryByName()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb", "Value");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

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
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"x:\mydirectory\directory1");
            fileSystem.CreateDirectory(@"x:\mydirectory\directory2\child");
            fileSystem.CreateFile(@"x:\mydirectory\directory3\file.rgb", "Value");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

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
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.Directory(a_name: null);
        }

        [TestMethod]
        public void GetDirectoryPath()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.DirectoryPath("Directory2");

            // Assert
            Assert.AreEqual(@"x:\mydirectory\Directory2", result);
        }


        [TestMethod]
        public void GetDirectoryPathForNotExistingChild()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.DirectoryPath("Directory4");

            // Assert
            Assert.AreEqual(@"x:\mydirectory\Directory4", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDirectoryPathWithNullName()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            directory.DirectoryPath(a_name: null);
        }

        [TestMethod]
        public void GetChildFileByName()
        {
            // Setup
            var created = DateTime.UtcNow;
            var lastModified = DateTime.UtcNow;
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat", "Value1");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat", "Value2");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat", "Value3");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat", "Value4");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.File("File1.dat");

            // Assert
            Assert.AreEqual("File1.dat", result.Name);
            Assert.IsTrue(result.Exists);
            Assert.AreEqual("Value1", result.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFileByNameWithNull()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            directory.File(a_name: null);
        }

        [TestMethod]
        public void GetFilePathByName()
        {
            // Setup
            var created = DateTime.UtcNow;
            var lastModified = DateTime.UtcNow;
            var fileSystem = new PathTree<string>();
            fileSystem.CreateFile(@"x:\mydirectory\file1.dat", "Value1");
            fileSystem.CreateFile(@"x:\mydirectory\file2.dat", "Value2");
            fileSystem.CreateFile(@"x:\mydirectory\file3.dat", "Value3");
            fileSystem.CreateFile(@"x:\mydirectory\otherdirectory\file4.dat", "Value4");
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            var result = directory.FilePath("File1.dat");

            // Assert
            Assert.AreEqual(@"x:\mydirectory\File1.dat", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDirectoryByNameWithNull()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, @"x:\mydirectory");

            // Execute
            directory.File(a_name: null);
        }

        [TestMethod]
        public void CreateDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            directory.Create();

            // Assert
            Assert.IsTrue(directory.Exists);
        }

        [TestMethod]
        public void DeleteDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\this\\is\\a\\directory");
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
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(fileSystem, "\\this\\is\\a\\directory");

            // Execute
            directory.Delete();
        }


        [TestMethod]
        public void EmptyDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory(@"\directory");
            fileSystem.CreateDirectory(@"\directory\child1");
            fileSystem.CreateDirectory(@"\directory\child2");
            fileSystem.CreateDirectory(@"\directory\child3");
            fileSystem.CreateFile(@"\directory\file1.dat", "Value1");
            fileSystem.CreateFile(@"\directory\file2.dat", "Value2");
            fileSystem.CreateFile(@"\directory\file3.dat", "Value3");
            fileSystem.CreateFile(@"\directory\child2\fileA.dat", "ValueA");
            fileSystem.CreateFile(@"\directory\child2\fileB.dat", "ValueB");
            fileSystem.CreateFile(@"\directory\child2\fileC.dat", "ValueC");

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
        public void CopyIn()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory(@"\directory");
            var file = fileSystem.CreateFile(@"\file1.dat", "Value");

            // Execute
            var result = directory.CopyIn(file);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Exists);
            Assert.AreEqual(@"\directory\file1.dat", result.Path);
            Assert.IsTrue(fileSystem.FileExists(@"\directory\file1.dat"));
            Assert.IsInstanceOfType(result, typeof (PathFile<string>));
            Assert.AreSame(fileSystem, (result as PathFile<string>)?.FileSystem);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyInWithNullFile()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory(@"\directory");
            fileSystem.CreateFile(@"\file1.dat", "Value");

            // Execute
            directory.CopyIn(a_file: null);
        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyInWithNotExistingFile()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory(@"\directory");
            var file = new PathFile<string>(fileSystem, @"\file1.dat");

            // Execute
            directory.CopyIn(file);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CopyInWithNotExistingDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = fileSystem.CreateDirectory(@"\directory");
            var file = new PathFile<string>(fileSystem, @"\noexisty\file1.dat");

            // Execute
            directory.CopyIn(file);
        }


        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void CopyInOnNotExistingDirectory()
        {
            // Setup
            var fileSystem = new PathTree<string>();
            var directory = new PathDirectory<string>(@"\directory");
            var file = fileSystem.CreateFile(@"\file1.dat", "Value");

            // Execute
            directory.CopyIn(file);
        }

        [TestMethod]
        public void CustomTest_I()
        {
            var fileSystem = new PathTree<string>();
            fileSystem.CreateDirectory(@"\Root\Directory");
            fileSystem.CreateDirectory(@"\Root\Directory\Sub1");
            fileSystem.CreateDirectory(@"\Root\Directory\Sub2");
            fileSystem.CreateDirectory(@"\Root\Directory\Sub3");
            fileSystem.CreateFile(@"\Root\Directory\File1.hsf", "Value");
            fileSystem.CreateFile(@"\Root\Directory\File2.hsf", "Value");
            fileSystem.CreateFile(@"\Root\Directory\File3.hsf", "Value");

            var directory = new PathDirectory<string>(fileSystem, @"\");

            var result = directory.Directory(@"Root\Directory\Sub1").Exists;

            Assert.IsTrue(result);
        }

    }
}