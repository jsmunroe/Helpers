using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Extensions
{
    [TestClass]
    public class CollectionHelpersTest
    {

        [TestMethod]
        public void AddRange()
        {
            // Setup
            var collection = new Collection<string>();
            
            // Execute
            collection.AddRange("One", "Two", "Three", "Four");

            // Assert
            CollectionAssert.AreEquivalent(new[] {"One", "Two", "Three", "Four"}, collection);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddRangeOnNull()
        {
            // Setup
            ICollection<int> collection = null;

            // Execute
            collection.AddRange(1, 2, 3, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRangeWithNull()
        {
            // Setup
            var collection = new Collection<string>();

            // Execute
            collection.AddRange(a_items: null);
        }


        [TestMethod]
        public void RemoveRange()
        {
            // Setup
            var collection = new Collection<string>();
            collection.AddRange("One", "Two", "Three", "Four");

            // Execute
            collection.RemoveRange("Two", "Three");

            // Assert
            CollectionAssert.AreEquivalent(new[] { "One", "Four" }, collection);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveRangeOnNull()
        {
            // Setup
            ICollection<int> collection = null;

            // Execute
            collection.RemoveRange(1, 2, 3, 4);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveRangeWithNull()
        {
            // Setup
            var collection = new Collection<string>();

            // Execute
            collection.RemoveRange(a_items: null);
        }

        [TestMethod]
        public void RemoveWhere()
        {
            // Setup
            var collection = new Collection<string>();
            collection.AddRange("One", "Two", "Three", "Four");

            // Execute
            collection.RemoveWhere(i => i.Length > 3);

            // Assert
            CollectionAssert.AreEquivalent(new[] { "One", "Two" }, collection);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemoveWhereOnNull()
        {
            // Setup
            ICollection<int> collection = null;

            // Execute
            collection.RemoveWhere(i => i > 5);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveWhereWithNull()
        {
            // Setup
            var collection = new Collection<string>();

            // Execute
            collection.RemoveWhere(a_predicate: null);
        }








    }
}
