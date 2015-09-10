using System;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using Helpers.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Xml
{
    [TestClass]
    public class XmlSetTest
    {
        #region Helper

        private SimpleType[] InitializeSequence()
        {
            return new []
            {
                new SimpleType {Name = "Apple", Price = 14.75m, Value=72.72 },
                new SimpleType {Name = "Orange", Price = 13.60m, Value=42.42 },
                new SimpleType {Name = "Banana", Price = 12.40m, Value=92.92 },
                new SimpleType {Name = "Fig", Price = 18.52m, Value=80.80 },
            };
        }


        private string GetXmlFile(string a_name) { return $@"Data\{a_name}.xml"; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            Directory.CreateDirectory("Data");
        } 

        #endregion

        [TestMethod]
        public void ConstructXmlSet()
        {
            // Setup
            var file = GetXmlFile(nameof(ConstructXmlSet));

            // Execute
            new XmlSet<SimpleType>(file);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructXmlSetWithNullFile()
        {
            // Execute
            new XmlSet<SimpleType>(a_filePath: null);
        }
        
        [TestMethod]
        public void AddEntity()
        {
            // Setup
            var file = GetXmlFile(nameof(AddEntity));
            var set = new XmlSet<SimpleType>(file);
            var simpleType = new SimpleType();

            // Execute
            set.Add(simpleType);

            // Assert
            Assert.AreSame(simpleType, set.First());
            Assert.AreEqual(1, set.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddEntityWithNull()
        {
            // Setup
            var file = GetXmlFile(nameof(AddEntityWithNull));
            var set = new XmlSet<SimpleType>(file);

            // Execute
            set.Add(a_entity: null);
        }
        
        [TestMethod]
        public void AddSimpleTypeSaveAndLoad()
        {
            // Setup
            var file = GetXmlFile(nameof(AddSimpleTypeSaveAndLoad));
            File.Delete(file);
            var set = new XmlSet<SimpleType>(file);

            // Execute
            var simple = new SimpleType
            {
                Name = "Test",
                Value = 12.35,
                Price = 4.50m,
                Date = DateTime.Parse("9/1/2015")
            };
            set.Add(simple);
            set.Save();

            var set2 = new XmlSet<SimpleType>(file);
            var result = set2.First();

            // Assert
            Assert.AreEqual(simple.Name, result.Name);
            Assert.AreEqual(simple.Value, result.Value);
            Assert.AreEqual(simple.Price, result.Price);
            Assert.AreEqual(simple.Date, result.Date);
        }

        [TestMethod]
        public void AddComplexTypeSaveAndLoad()
        {
            // Setup
            var file = GetXmlFile(nameof(AddComplexTypeSaveAndLoad));
            File.Delete(file);
            var set = new XmlSet<ComplexType>(file);

            // Execute
            var complex = new ComplexType
            {
                Description = "This is a test",
                Size = 42,
                Child = new SimpleType
                {
                    Name = "Test",
                    Value = 12.35,
                    Price = 4.50m,
                    Date = DateTime.Parse("9/1/2015")
                }
            };
            set.Add(complex);
            set.Save();

            var set2 = new XmlSet<ComplexType>(file);
            var result = set2.First();

            // Assert
            Assert.AreEqual(complex.Description, result.Description);
            Assert.AreEqual(complex.Size, result.Size);
            Assert.IsNotNull(complex.Child);
            Assert.AreEqual(complex.Child.Name, result.Child.Name);
            Assert.AreEqual(complex.Child.Value, result.Child.Value);
            Assert.AreEqual(complex.Child.Price, result.Child.Price);
            Assert.AreEqual(complex.Child.Date, result.Child.Date);
        }


        [TestMethod]
        public void RevertSet()
        {
            // Setup up.
            var file = GetXmlFile(nameof(RevertSet));
            File.Delete(file);
            var set = new XmlSet<SimpleType>(file);

            var simple = new SimpleType
            {
                Name = "Test",
                Value = 12.35,
                Price = 4.50m,
                Date = DateTime.Parse("9/1/2015")
            };
            set.Add(simple);
            set.Save();

            var simple2 = new SimpleType
            {
                Name = "Test 2",
                Value = 43.001,
                Price = 8.5m,
                Date = DateTime.Parse("9/2/2015")
            };

            // Execute
            set.Revert();

            // Assert
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(simple.Name, set.First().Name);
        }



        [TestMethod]
        public void RemoveEntity()
        {
            // Setup
            var sequence = InitializeSequence();
            var removedItem = sequence[1];
            var set = new TestSet<SimpleType>(sequence);

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
            var set = new TestSet<SimpleType>(sequence);

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
            var removedItem = new SimpleType { Name = "NoExisty", Price = 0m, Value = 1000000 };
            var set = new TestSet<SimpleType>(sequence);

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
            var set = new TestSet<SimpleType>(sequence);

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
            var set = new TestSet<SimpleType>(sequence);

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
            var item = new SimpleType { Name = "ImNotReal", Price = 2.22m, Value = 2 };
            var set = new TestSet<SimpleType>(sequence);

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
            var set = new TestSet<SimpleType>(sequence);
            var array = new SimpleType[4];

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
            var set = new TestSet<SimpleType>(sequence);
            var array = new SimpleType[5];

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
















        class SimpleType
        {
            public string Name { get; set; }
            public double Value { get; set; }
            public decimal Price { get; set; }
            public DateTime Date { get; set; }
        }

        class ComplexType
        {
            public string Description { get; set; }
            public int Size { get; set; }
            public SimpleType Child { get; set; }
        }
    }
}
