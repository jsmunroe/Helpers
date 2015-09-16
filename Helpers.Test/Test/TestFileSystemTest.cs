using System;
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
        public void RegisterDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterDirectoryWithNullPath()
        {
            Assert.Inconclusive("Not implemented!");
        }

        [TestMethod]
        public void ResolveDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveDirectoryWithNullPath()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void ResolveNotExistingDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }
        
        [TestMethod]
        public void RegisterFile()
        {
            Assert.Inconclusive("Not implemented!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterFileWithNullPath()
        {
            Assert.Inconclusive("Not implemented!");
        }
        
        [TestMethod]
        public void ResolveFile()
        {
            Assert.Inconclusive("Not implemented!");
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveFileWithNullPath()
        {
            Assert.Inconclusive("Not implemented!");
        }

        [TestMethod]
        public void ResolveNotExistingFile()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void ContainsDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void ContainsDirectoryWithNotExistingDirectory()
        {
            Assert.Inconclusive("Not implemented!");
        }


        [TestMethod]
        public void ContainsFile()
        {
            Assert.Inconclusive("Not implemented!");
        }
        
        [TestMethod]
        public void ContainsFileWithNotExistingFile()
        {
            Assert.Inconclusive("Not implemented!");
        }
    }
}
