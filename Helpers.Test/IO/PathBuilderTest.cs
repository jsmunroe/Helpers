﻿using System;
using Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.IO
{
    [TestClass]
    public class PathBuilderTest
    {
        [TestMethod]
        public void Construct()
        {
            // Execute
            var result = new PathBuilder(@"\some\odd\path");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(@"\some\odd\path", result.ToString());
        }


        [TestMethod]
        public void ConstructWithCustomDelimiter()
        {
            // Execute
            var result = new PathBuilder(@":some:odd:path", ":");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(@":some:odd:path", result.ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullPath()
        {
            // Execute
            var result = new PathBuilder(a_path: null, a_delimiter:":");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullDelimiter()
        {
            // Execute
            var result = new PathBuilder(a_path: @"\some\odd\path", a_delimiter: null);
        }

        [TestMethod]
        public void Child()
        {
            // Setup
            var path = PathBuilder.Create(@"\some\odd\path");

            // Execute
            var result = path.Child("next");

            // Assert
            Assert.AreEqual(@"\some\odd\path\next", result);
        }


        [TestMethod]
        public void ChildOffOfRoot()
        {
            // Setup
            var path = PathBuilder.Create(@"C:\");

            // Execute
            var result = path.Child("next");

            // Assert
            Assert.AreEqual(@"C:\next", result);
        }

        [TestMethod]
        public void ChildWithCustomDelimiter()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Child("next");

            // Assert
            Assert.AreEqual(@":some:odd:path:next", result);
        }


        [TestMethod]
        public void ChildWithEmptyRelativePath()
        {
            // Setup
            var path = PathBuilder.Create(@"\some\odd\path");

            // Execute
            var result = path.Child("");

            // Assert
            Assert.AreEqual(@"\some\odd\path", result);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChildWithNullRelativePath()
        {
            // Setup
            var path = PathBuilder.Create(@"\some\odd\path");

            // Execute
            var result = path.Child(a_relativePath: null);
        }


        [TestMethod]
        public void ChildOffOfExplicitRoot()
        {
            // Setup
            var path = PathBuilder.Create(@"C:\").WithRoot(PathBuilder.WindowsDriveRoot);

            // Execute
            var result = path.Child("next");

            // Assert
            Assert.AreEqual(@"C:\next", result);
        }


        [TestMethod]
        public void ChildOffOfEmpty()
        {
            // Setup
            var path = PathBuilder.Create(@"");

            // Execute
            var result = path.Child("next");

            // Assert
            Assert.AreEqual(@"next", result);
        }


        [TestMethod]
        public void Sibling()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Sibling("next");

            // Assert
            Assert.AreEqual(@":some:odd:next", result);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SiblingWithNullName()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Sibling(a_siblingName: null);
        }


        [TestMethod]
        public void SiblingWithEmpty()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Sibling("");

            // Assert
            Assert.AreEqual(@":some:odd", result);
        }


        [TestMethod]
        public void SiblingWithRelativePath()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Sibling("another:path");

            // Assert
            Assert.AreEqual(@":some:odd:another:path", result);
        }


        [TestMethod]
        public void SiblingWithRelativePathStartingWithDelimiter()
        {
            // Setup
            var path = PathBuilder.Create(@"#some#odd#path", "#");

            // Execute
            var result = path.Sibling("#another");

            // Assert
            Assert.AreEqual(@"#some#odd#another", result);
        }


        [TestMethod]
        public void SiblingsWithRelativePathOfOnlyDelimiter()
        {
            // Setup
            var path = PathBuilder.Create(@"#some#odd#path", "#");

            // Execute
            var result = path.Sibling("#");

            // Assert
            Assert.AreEqual(@"#some#odd", result);
        }

        [TestMethod]
        public void Parent()
        {
            // Setup
            var path = PathBuilder.Create(@"\some\odd\path");

            // Execute
            var result = path.Parent();

            // Assert
            Assert.AreEqual(@"\some\odd", result);
        }


        [TestMethod]
        public void ParentFromRoot_I()
        {
            // Setup
            var path = PathBuilder.Create(@"\");

            // Execute
            var result = path.Parent();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParentFromRoot_II()
        {
            // Setup
            var path = PathBuilder.Create(@"root");

            // Execute
            var result = path.Parent();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParentFromEmpty()
        {
            // Setup
            var path = PathBuilder.Create(@"");

            // Execute
            var result = path.Parent();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParentWithCustomDelimiter()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Parent();

            // Assert
            Assert.AreEqual(@":some:odd", result);
        }


        [TestMethod]
        public void Name()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Name();

            // Assert
            Assert.AreEqual("path", result);
        }

        [TestMethod]
        public void NameFromRoot_I()
        {
            // Setup
            var path = PathBuilder.Create(@"\");

            // Execute
            var result = path.Name();

            // Assert
            Assert.AreEqual(@"", result);
        }

        [TestMethod]
        public void NameFromRoot_II()
        {
            // Setup
            var path = PathBuilder.Create(@"root");

            // Execute
            var result = path.Name();

            // Assert
            Assert.AreEqual(@"root", result);
        }

        [TestMethod]
        public void NameFromEmpty()
        {
            // Setup
            var path = PathBuilder.Create(@"");

            // Execute
            var result = path.Name();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void NameWithCustomDelimiter()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path", ":");

            // Execute
            var result = path.Name();

            // Assert
            Assert.AreEqual(@"path", result);
        }

        [TestMethod]
        public void NameWithoutExtension()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path.txt", ":");

            // Execute
            var result = path.NameWithoutExtension();

            // Assert
            Assert.AreEqual("path", result);
        }

        [TestMethod]
        public void NameFromRootWithoutExtension_I()
        {
            // Setup
            var path = PathBuilder.Create(@"\");

            // Execute
            var result = path.NameWithoutExtension();

            // Assert
            Assert.AreEqual(@"", result);
        }

        [TestMethod]
        public void NameFromRootWithoutExtension_II()
        {
            // Setup
            var path = PathBuilder.Create(@"root.dat");

            // Execute
            var result = path.NameWithoutExtension();

            // Assert
            Assert.AreEqual(@"root", result);
        }

        [TestMethod]
        public void NameWithoutExtensionFromEmpty()
        {
            // Setup
            var path = PathBuilder.Create(@"");

            // Execute
            var result = path.NameWithoutExtension();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void NameWithoutExtensionWithCustomDelimiter()
        {
            // Setup
            var path = PathBuilder.Create(@":some:odd:path.jpg", ":");

            // Execute
            var result = path.NameWithoutExtension();

            // Assert
            Assert.AreEqual(@"path", result);
        }


        [TestMethod]
        public void Temp()
        {
            Assert.AreEqual("filename.txt", System.IO.Path.GetFileNameWithoutExtension("filename.txt.dat"));
        }


    }
}