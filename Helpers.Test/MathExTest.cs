using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Helpers.Test
{
    [TestClass]
    public class MathExTest
    {
        [TestMethod]
        public void Max_I()
        {
            // Execute
            var result = MathEx.Max(100, 10);

            // Assert
            Assert.AreEqual(100, result);
        }


        [TestMethod]
        public void Max_II()
        {
            // Setup
            var mockComparable2 = new Mock<IComparable>();
            var mockComparable1 = new Mock<IComparable>();
            mockComparable1.Setup(i => i.CompareTo(mockComparable2.Object)).Returns(1);
            mockComparable2.Setup(i => i.CompareTo(mockComparable1.Object)).Returns(-1);

            // Execute
            var result1 = MathEx.Max(mockComparable1.Object, mockComparable2.Object);
            var result2 = MathEx.Max(mockComparable2.Object, mockComparable1.Object);

            // Assert
            Assert.AreEqual(mockComparable1.Object, result1);
            Assert.AreEqual(mockComparable1.Object, result2);
        }


        [TestMethod]
        public void MaxWithEqualValues() // Returns first.
        {
            // Setup
            var mockComparable2 = new Mock<IComparable>();
            var mockComparable1 = new Mock<IComparable>();
            mockComparable1.Setup(i => i.CompareTo(mockComparable2.Object)).Returns(0);
            mockComparable2.Setup(i => i.CompareTo(mockComparable1.Object)).Returns(0);

            // Execute
            var result1 = MathEx.Max(mockComparable1.Object, mockComparable2.Object);
            var result2 = MathEx.Max(mockComparable2.Object, mockComparable1.Object);

            // Assert
            Assert.AreEqual(mockComparable1.Object, result1);
            Assert.AreEqual(mockComparable2.Object, result2);
        }

        [TestMethod]
        public void MaxWithNullFirst()
        {
            // Setup
            var mockComparable = new Mock<IComparable>();

            // Execute
            var result1 = MathEx.Max(a_first: null, a_second: mockComparable.Object);

            // Assert
            Assert.AreEqual(mockComparable.Object, result1);
        }

        [TestMethod]
        public void MaxWithNullSecond()
        {
            // Setup
            var mockComparable = new Mock<IComparable>();

            // Execute
            var result1 = MathEx.Max(a_first: mockComparable.Object, a_second: null);

            // Assert
            Assert.AreEqual(mockComparable.Object, result1);
        }


        [TestMethod]
        public void MaxWithBothNull()
        {
            // Execute
            var result1 = MathEx.Max<IComparable>(a_first: null, a_second: null);

            // Assert
            Assert.IsNull(result1);
        }

        [TestMethod]
        public void Min_I()
        {
            // Execute
            var result = MathEx.Min(100, 10);

            // Assert
            Assert.AreEqual(10, result);
        }


        [TestMethod]
        public void Min_II()
        {
            // Setup
            var mockComparable2 = new Mock<IComparable>();
            var mockComparable1 = new Mock<IComparable>();
            mockComparable1.Setup(i => i.CompareTo(mockComparable2.Object)).Returns(1);
            mockComparable2.Setup(i => i.CompareTo(mockComparable1.Object)).Returns(-1);

            // Execute
            var result1 = MathEx.Min(mockComparable1.Object, mockComparable2.Object);
            var result2 = MathEx.Min(mockComparable2.Object, mockComparable1.Object);

            // Assert
            Assert.AreEqual(mockComparable2.Object, result1);
            Assert.AreEqual(mockComparable2.Object, result2);
        }


        [TestMethod]
        public void MinWithEqualValues() // Returns first.
        {
            // Setup
            var mockComparable2 = new Mock<IComparable>();
            var mockComparable1 = new Mock<IComparable>();
            mockComparable1.Setup(i => i.CompareTo(mockComparable2.Object)).Returns(0);
            mockComparable2.Setup(i => i.CompareTo(mockComparable1.Object)).Returns(0);

            // Execute
            var result1 = MathEx.Min(mockComparable1.Object, mockComparable2.Object);
            var result2 = MathEx.Min(mockComparable2.Object, mockComparable1.Object);

            // Assert
            Assert.AreEqual(mockComparable1.Object, result1);
            Assert.AreEqual(mockComparable2.Object, result2);
        }

        [TestMethod]
        public void MinWithNullFirst()
        {
            // Setup
            var mockComparable = new Mock<IComparable>();

            // Execute
            var result = MathEx.Min(a_first: null, a_second: mockComparable.Object);

            // Assert
            Assert.AreEqual(mockComparable.Object, result);
        }

        [TestMethod]
        public void MinWithNullSecond()
        {
            // Setup
            var mockComparable = new Mock<IComparable>();

            // Execute
            var result = MathEx.Min(a_first: mockComparable.Object, a_second: null);

            // Assert
            Assert.AreEqual(mockComparable.Object, result);
        }


        [TestMethod]
        public void MinWithBothNull()
        {
            // Execute
            var result1 = MathEx.Min<IComparable>(a_first: null, a_second: null);

            // Assert
            Assert.IsNull(result1);
        }

    }
}
