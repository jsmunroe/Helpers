using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Extensions
{
    [TestClass]
    public class DirectoryHelpersTest
    {
        private TestFileSystem _fileSystem;

        [TestInitialize]
        public void InitializeTest()
        {
            _fileSystem = new TestFileSystem();
            _fileSystem.StageDirectory(@"\Root\Directory");
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1");
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1\Sub2");
            _fileSystem.StageFile(@"\Root\Directory\Sub1\Sub2\File1.dat");
            _fileSystem.StageFile(@"\Root\Directory\Sub1\Sub2\File2.dat");
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1\Sub3");
            _fileSystem.StageFile(@"\Root\Directory\Sub1\Sub3\File1.dat");
            _fileSystem.StageFile(@"\Root\Directory\Sub1\Sub3\File2.dat");
            _fileSystem.StageFile(@"\Root\Directory\Sub1\File1.dat");
            _fileSystem.StageFile(@"\Root\Directory\Sub1\File2.dat");
        }


        [TestMethod]
        public void GetDescendentDirectories()
        {
            // Setup
            var source = new TestDirectory(_fileSystem, @"\Root\Directory");

            // Execute
            var result = source.GetDescendentDirectories();

            // Assert
            Assert.AreEqual(3, result.Length);
            CollectionAssert.AreEquivalent(new [] {"Sub1", "Sub2", "Sub3"}, result.Select(i => i.Name).ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetDescendentDirectoriesOnNull()
        {
            // Setup
            TestDirectory source = null;

            // Execute
            var result = source.GetDescendentDirectories();
        }


        [TestMethod]
        public void GetDescendentDirectoriesWhereNoneExist()
        {
            // Setup
            var source = new TestDirectory(_fileSystem, @"\Root\Directory\sub1\sub3");

            // Execute
            var result = source.GetDescendentDirectories();

            // Assert
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public void GetDescendentFiles()
        {
            // Setup
            var source = new TestDirectory(_fileSystem, @"\Root\Directory");

            // Execute
            var result = source.GetDescendentFiles();

            // Assert
            Assert.AreEqual(6, result.Length);
            CollectionAssert.AreEquivalent(new[] { "File1.dat", "File1.dat", "File1.dat", "File2.dat", "File2.dat", "File2.dat", }, result.Select(i => i.Name).ToArray());
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetDescendentFilesOnNull()
        {
            // Setup
            TestDirectory source = null;

            // Execute
            var result = source.GetDescendentFiles();
        }


        [TestMethod]
        public void GetDescendentFilesWhereNoneExist()
        {
            // Setup
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1\Sub4");
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1\Sub4\sub5");
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1\Sub4\sub6");
            _fileSystem.StageDirectory(@"\Root\Directory\Sub1\Sub4\sub6\sub7");
            var source = new TestDirectory(_fileSystem, @"\Root\Directory\sub1\sub4");

            // Execute
            var result = source.GetDescendentFiles();

            // Assert
            Assert.AreEqual(0, result.Length);
        }



        [TestMethod]
        public void CopyTo()
        {
            // Setup
            var source = new TestDirectory(_fileSystem, @"\Root\Directory\Sub1");
            var dest = new TestDirectory(_fileSystem, @"\Root\Directory\SubA");

            // Execute
            DirectoryHelpers.CopyTo(source, dest);

            // Assert
            Assert.IsTrue(_fileSystem.DirectoryExists(@"\Root\Directory\SubA"));
            Assert.IsTrue(_fileSystem.DirectoryExists(@"\Root\Directory\SubA\Sub2"));
            Assert.IsTrue(_fileSystem.FileExists(@"\Root\Directory\SubA\Sub2\File1.dat"));
            Assert.IsTrue(_fileSystem.FileExists(@"\Root\Directory\SubA\Sub2\File2.dat"));
            Assert.IsTrue(_fileSystem.DirectoryExists(@"\Root\Directory\SubA\Sub3"));
            Assert.IsTrue(_fileSystem.FileExists(@"\Root\Directory\SubA\Sub3\File1.dat"));
            Assert.IsTrue(_fileSystem.FileExists(@"\Root\Directory\SubA\Sub3\File2.dat"));
            Assert.IsTrue(_fileSystem.FileExists(@"\Root\Directory\SubA\File1.dat"));
            Assert.IsTrue(_fileSystem.FileExists(@"\Root\Directory\SubA\File2.dat"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToWithNullDirectory()
        {
            // Setup
            var source = new TestDirectory(_fileSystem, @"\Root\Directory\Sub1");

            // Execute
            DirectoryHelpers.CopyTo(a_this: source, a_destination: null);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CopyToWithNullThis()
        {
            // Setup
            var dest = new TestDirectory(_fileSystem, @"\Root\Directory\SubA");

            // Execute
            DirectoryHelpers.CopyTo(a_this: null, a_destination: dest);
        }
    }
}
