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
        private string GetXmlFile(string a_name) {  return $@"Data\{a_name}.xml"; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            Directory.CreateDirectory("Data");
        }

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
            var file = GetXmlFile(nameof(AddEntity));
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
