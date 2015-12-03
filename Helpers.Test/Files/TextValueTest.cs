using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Files
{
    [TestClass]
    public class TextValueTest
    {
        [TestMethod]
        public void Construct()
        {
            // Execute
            var textValue = new TextValue("Value");

            // Assert
            Assert.AreEqual("Value", textValue.ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNull()
        {
            // Execute
            new TextValue(a_value: null);
        }


        [TestMethod]
        public void CastToString()
        {
            // Setup
            var textValue = new TextValue("Value");

            // Execute
            var value = (string) textValue;

            // Assert
            Assert.AreEqual("Value", value);
        }


        [TestMethod]
        public void CastFromString()
        {
            // Execute
            TextValue textValue = "Value";

            // Assert
            Assert.AreEqual("Value", textValue.ToString());
        }


        [TestMethod]
        public void CastToDouble()
        {
            // Setup
            var textValue = new TextValue("9.8");

            // Execute
            var value = (double) textValue;

            // Assert
            Assert.AreEqual(9.8, value);
        }


        [TestMethod]
        public void CastNonDoubleToDouble()
        {
            // Setup
            var textValue = new TextValue("Meow!");

            // Execute
            var value = (double)textValue;

            // Assert
            Assert.AreEqual(0, value);
        }


        [TestMethod]
        public void CastFromDouble()
        {
            // Execute
            TextValue textValue = 104.208;

            // Assert
            Assert.AreEqual("104.208", textValue.ToString());
        }


        [TestMethod]
        public void CastToInt()
        {
            // Setup
            var textValue = new TextValue("98");

            // Execute
            var value = (int)textValue;

            // Assert
            Assert.AreEqual(98, value);
        }


        [TestMethod]
        public void CastNonIntToInt()
        {
            // Setup
            var textValue = new TextValue("Ninety-eight");

            // Execute
            var value = (int)textValue;

            // Assert
            Assert.AreEqual(0, value);
        }


        [TestMethod]
        public void CastDoubleValueToInt()
        {
            // Setup
            var textValue = new TextValue("9.8");

            // Execute
            var value = (int)textValue;

            // Assert
            Assert.AreEqual(0, value);
        }

        [TestMethod]
        public void CastFromInt()
        {
            // Execute
            TextValue textValue = 104208;

            // Assert
            Assert.AreEqual("104208", textValue.ToString());
        }

        [TestMethod]
        public void CastTrueToBool()
        {
            // Setup
            var textValue = new TextValue("tRUE");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsTrue(value);
        }


        [TestMethod]
        public void CastFalseToBool()
        {
            // Setup
            var textValue = new TextValue("faLse");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsFalse(value);
        }


        [TestMethod]
        public void Cast1ToBool()
        {
            // Setup
            var textValue = new TextValue("1");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void Cast0ToBool()
        {
            // Setup
            var textValue = new TextValue("0");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void CastYesToBool()
        {
            // Setup
            var textValue = new TextValue("yEs");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsTrue(value);
        }


        [TestMethod]
        public void CastNoToBool()
        {
            // Setup
            var textValue = new TextValue("nO");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsFalse(value);
        }


        [TestMethod]
        public void CastNonBoolToBool()
        {
            // Setup
            var textValue = new TextValue("E=mc^2");

            // Execute
            var value = (bool)textValue;

            // Assert
            Assert.IsFalse(value);
        }


        [TestMethod]
        public void CastFromTrue()
        {
            // Setup
            TextValue textValue = true;

            // Assert
            Assert.AreEqual("true", textValue.ToString());
        }


        [TestMethod]
        public void CastFromFalse()
        {
            // Setup
            TextValue textValue = false;

            // Assert
            Assert.AreEqual("false", textValue.ToString());
        }



    }
}