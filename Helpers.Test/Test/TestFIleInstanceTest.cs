using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Test
{
    [TestClass]
    public class TestFileInstanceTest
    {

        [TestMethod]
        public void Construct()
        {
            // Execute
            new TestFileInstance();
        }


        [TestMethod]
        public void ConstructWithStringData()
        {
            // Execute
            var instance = new TestFileInstance("This is some data.", Encoding.UTF8);

            // Assert
            var data = Encoding.UTF8.GetString(instance.Data);
            Assert.AreEqual("This is some data.", data);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullStringData()
        {
            new TestFileInstance(a_data: null, a_encoding: Encoding.UTF8);
        }


        [TestMethod]
        public void ConstructWithStringDataAndNullEncoding() // Defaults to UTF8
        {
            // Execute
            var instance = new TestFileInstance("This is some data.", a_encoding: null);

            // Assert
            var data = Encoding.UTF8.GetString(instance.Data);
            Assert.AreEqual("This is some data.", data);
        }


        [TestMethod]
        public void ConstructWithBinaryData()
        {
            // Setup
            var data = Encoding.UTF8.GetBytes("This is data too.");

            // Execute
            var instance = new TestFileInstance(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullBinaryData()
        {
            new TestFileInstance(a_data: null);
        }




    }
}