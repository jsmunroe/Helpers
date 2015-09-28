using System;
using System.Collections.Generic;
using Helpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Extensions
{
    [TestClass]
    public class DictionaryHelpersTest
    {
        [TestMethod]
        public void GetValue()
        {
            // Setup
            var dictionary = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            // Execute
            var value = dictionary.GetValueWithDefault("one", 50);

            // Assert
            Assert.AreEqual(1, value);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetValueWithNullThis()
        {
            // Setup
            var dictionary = (Dictionary<string, int>)null;

            // Execute
            var value = dictionary.GetValueWithDefault("one", 50);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValueWithNullKey()
        {
            // Setup
            var dictionary = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3},
            };

            // Execute
            var value = dictionary.GetValueWithDefault(a_key: null, a_default: 50);
        }


        [TestMethod]
        public void GetValueWithNotExistingKey()
        {
            // Setup
            var dictionary = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            // Execute
            var value = dictionary.GetValueWithDefault("1", 50);

            // Assert
            Assert.AreEqual(50, value);
        }
    }
}
