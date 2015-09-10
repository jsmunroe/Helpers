using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Test
{
    [TestClass]
    public class TestSetTest
    {
        #region Helpers

        private TestEntity[] InitializeSequence()
        {
            return new TestEntity[]
            {
                new TestEntity {Name = "Apple", Price = 14.75m, Quantity=72 },
                new TestEntity {Name = "Orange", Price = 13.60m, Quantity=42 },
                new TestEntity {Name = "Banana", Price = 12.40m, Quantity=92 },
                new TestEntity {Name = "Fig", Price = 18.52m, Quantity=80 },
            };
        } 

        #endregion

        [TestMethod]
        public void ConstructTestSetWithSequence()
        {
            // Setup
            var sequence = InitializeSequence();

            // Execute
            var set = new TestSet<TestEntity>(InitializeSequence());

            // Assert
            Assert.AreEqual(sequence.Length, set.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructTestWithNullSequence()
        {
            // Execute
            var set = new TestSet<TestEntity>(a_items: null);
        }


        [TestMethod]
        public void AddEntity()
        {
            // Setup
            var item = InitializeSequence()[0];
            var set = new TestSet<TestEntity>();

            // Execute
            set.Add(item);

            // Assert
            Assert.AreEqual(1, set.Count);
            Assert.AreSame(item, set.First());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEntityWithNull()
        {
            // Setup
            var set = new TestSet<TestEntity>();

            // Execute
            set.Add(a_item: null);
        }


        [TestMethod]
        public void RemoveEntity()
        {
            // Setup
            var sequence = InitializeSequence();
            var removedItem = sequence[1];
            var set = new TestSet<TestEntity>(sequence);

            // Execute
            var result = set.Remove(removedItem);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(sequence.Length - 1, set.Count);
        }


        [TestMethod]
        public void RemoveNull()
        {
            // Setup
            var sequence = InitializeSequence();
            var set = new TestSet<TestEntity>(sequence);

            // Execute
            var result = set.Remove(a_entity: null);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(sequence.Length, set.Count);
        }


        [TestMethod]
        public void RemoveNonexistingEntity()
        {
            // Setup
            var sequence = InitializeSequence();
            var removedItem = new TestEntity { Name="NoExisty", Price=0m, Quantity=1000000 };
            var set = new TestSet<TestEntity>(sequence);

            // Execute
            var result = set.Remove(removedItem);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(sequence.Length, set.Count);
        }


        [TestMethod]
        public void Clear()
        {
            // Setup
            var sequence = InitializeSequence();
            var set = new TestSet<TestEntity>(sequence);

            // Execute
            set.Clear();

            // Assert
            Assert.IsFalse(set.Any());
            Assert.AreEqual(0, set.Count);
        }


        [TestMethod]
        public void ContainsExistingEntity()
        {
            // Setup
            var sequence = InitializeSequence();
            var item = sequence[3];
            var set = new TestSet<TestEntity>(sequence);

            // Execute
            var result = set.Contains(item);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsNonexistingEntity()
        {
            // Setup
            var sequence = InitializeSequence();
            var item = new TestEntity { Name = "ImNotReal", Price = 2.22m, Quantity = 2 };
            var set = new TestSet<TestEntity>(sequence);

            // Execute
            var result = set.Contains(item);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CopyTo()
        {
            // Setup
            var sequence = InitializeSequence();
            var set = new TestSet<TestEntity>(sequence);
            var array = new TestEntity[4];

            // Execute
            set.CopyTo(array, 0);

            // Assert
            Assert.IsTrue(array.All(i => i != null));
            Assert.AreSame(sequence[0], array[0]);
            Assert.AreSame(sequence[1], array[1]);
            Assert.AreSame(sequence[2], array[2]);
            Assert.AreSame(sequence[3], array[3]);
        }

        [TestMethod]
        public void CopyToAtNonZeroArrayIndex()
        {
            // Setup
            var sequence = InitializeSequence();
            var set = new TestSet<TestEntity>(sequence);
            var array = new TestEntity[5];

            // Execute
            set.CopyTo(array, 1);

            // Assert
            Assert.IsNull(array[0]);
            Assert.IsTrue(array.Skip(1).All(i => i != null));
            Assert.AreSame(sequence[0], array[1]);
            Assert.AreSame(sequence[1], array[2]);
            Assert.AreSame(sequence[2], array[3]);
            Assert.AreSame(sequence[3], array[4]);
        }
































        class TestEntity
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
