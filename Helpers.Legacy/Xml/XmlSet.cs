using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Helpers.Contracts;
using Helpers.IO;

namespace Helpers.Xml
{
    public class XmlSet<TEntity> : IFileSet<TEntity>
    {
        private int _version = 0;
        private readonly string _filePath;
        private List<TEntity> _entities = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_filePath">PathResult to the XML document file.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_filePath"/> is null.</exception>
        public XmlSet(string a_filePath)
        {
            #region Argument Validation

            if (a_filePath == null)
                throw new ArgumentNullException(nameof(a_filePath));

            #endregion

            _filePath = a_filePath;
        }

        /// <summary>
        /// Gets the number of elements contained in this set.
        /// </summary>
        /// <returns>
        /// The number of elements contained in this set.
        /// </returns>
        public int Count => Entities.Count;

        /// <summary>
        /// Gets a value indicating whether this set is read-only.
        /// </summary>
        /// <returns>
        /// true if this set is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => false;

        /// <summary>
        /// Entities within this set (protected).
        /// </summary>
        protected List<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = LoadEntities();

                return _entities;
            }
        }

        /// <summary>
        /// Add the given entity (<paramref name="a_entity"/>) to this set.
        /// </summary>
        /// <param name="a_entity">Element to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_entity"/> is null.</exception>
        public void Add(TEntity a_entity)
        {
            #region Argument Validation

            if (a_entity == null)
                throw new ArgumentNullException(nameof(a_entity));

            #endregion

            Entities.Add(a_entity);
        }

        /// <summary>
        /// Removes all items from this set.
        /// </summary>
        /// <exception cref="NotSupportedException">This set is read-only. </exception>
        public void Clear()
        {
            Entities.Clear();
        }

        /// <summary>
        /// Determines whether this set contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="a_item"/> is found in this set; otherwise, false.
        /// </returns>
        /// <param name="a_item">The object to locate in this set.</param>
        public bool Contains(TEntity a_item)
        {
            return Entities.Contains(a_item);
        }

        /// <summary>
        /// Copies the elements of this set to the given array (<paramref name="a_array"/>), starting at a given array index (<paramref name="a_arrayIndex"/>).
        /// </summary>
        /// <param name="a_array">The one-dimensional array that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The array must have zero-based indexing.</param><param name="a_arrayIndex">The zero-based index in <paramref name="a_array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="a_array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="a_arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="a_arrayIndex"/> to the end of the destination <paramref name="a_array"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_array"/> is null.</exception>
        public void CopyTo(TEntity[] a_array, int a_arrayIndex)
        {
            #region Argument Validation

            if (a_array == null)
                throw new ArgumentNullException(nameof(a_array));

            #endregion

            Entities.CopyTo(a_array, a_arrayIndex);
        }

        /// <summary>
        /// Remove the given entity (<paramref name="a_entity"/>) from this set.
        /// </summary>
        /// <param name="a_entity">Element to remove.</param>
        /// <returns>True if element was removed.</returns>
        public bool Remove(TEntity a_entity)
        {
            return Entities.Remove(a_entity);
        }

        /// <summary>
        /// Save changes in this set to its corresponding document.
        /// </summary>
        public void Save()
        {
            var directory = PathBuilder.Create(_filePath).Parent() ?? "";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            XDocument document;
            if (File.Exists(_filePath))
                document = XDocument.Load(_filePath);
            else
                document = CreateEmptyDocument();

            var xParent = GetParentElement(document);

            xParent.RemoveAll();
            xParent.Add(new XAttribute("LastSave", DateTime.Now));

            foreach (var entity in Entities)
            {
                var xElement = new XElement(typeof(TEntity).Name);
                SaveEntity(entity, xElement);
                xParent.Add(xElement);
            }

            document.Save(_filePath);
        }

        /// <summary>
        /// Revert the set back to its file.
        /// </summary>
        public void Revert()
        {
            _entities = LoadEntities();
        }

        /// <summary>
        /// Load the entities from the document at the end of the path herein.
        /// </summary>
        /// <returns>Loaded entities.</returns>
        private List<TEntity> LoadEntities()
        {
            if (!File.Exists(_filePath))
                return new List<TEntity>();

            var document = XDocument.Load(_filePath);

            var xParent = GetParentElement(document);

            var entities = new List<TEntity>();

            foreach (var xElement in xParent.Elements(typeof(TEntity).Name))
            {
                var entity = CreateEntity();
                LoadEntity(xElement, entity);
                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// Load the given entity (<paramref name="a_target"/>) from the given element (<paramref name="a_source"/>).
        /// </summary>
        /// <param name="a_source">Source element.</param>
        /// <param name="a_target">Target entity.</param>
        private void LoadEntity(XElement a_source, object a_target)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(a_target.GetType()))
            {
                if (property.IsReadOnly)
                    continue;

                var xProperty = a_source.Element(property.Name);

                if (xProperty == null)
                    continue;

                var text = (string)xProperty;

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(a_target, text);
                }
                else if (property.PropertyType.IsValueType)
                {
                    if (property.Converter != null && property.Converter.IsValid(text))
                    {
                        var value = property.Converter.ConvertFrom(text);
                        property.SetValue(a_target, value);
                    }
                }
                else
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    LoadEntity(xProperty, value);
                    property.SetValue(a_target, value);
                }
            }
        }

        /// <summary>
        /// Save the given entity (<paramref name="a_target"/>) to the given element (<paramref name="a_source"/>).
        /// </summary>
        /// <param name="a_source">Source entity.</param>
        /// <param name="a_target">Target entity.</param>
        private void SaveEntity(object a_source, XElement a_target)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(a_source.GetType()))
            {
                var value = property.GetValue(a_source);

                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                {
                    a_target.Add(new XElement(property.Name, value));
                }
                else
                {
                    var child = new XElement(property.Name);
                    SaveEntity(value, child);

                    a_target.Add(child);
                }
            }
        }

        /// <summary>
        /// Create an empty entity.
        /// </summary>
        /// <returns>Created entity.</returns>
        protected virtual TEntity CreateEntity()
        {
            return Activator.CreateInstance<TEntity>();
        }

        /// <summary>
        /// Get the parent element of the entities from within the given document (<paramref name="a_document"/>).
        /// </summary>
        /// <returns>Parent element.</returns>
        protected virtual XElement GetParentElement(XDocument a_document)
        {
            return a_document.Root;
        }

        /// <summary>
        /// Create an empty document.
        /// </summary>
        /// <returns>Created empty document.</returns>
        protected virtual XDocument CreateEmptyDocument()
        {
            var rootElementName = PathBuilder.Create(_filePath).NameWithoutExtension() ?? typeof(TEntity).Name + "Set";

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(rootElementName,
                    new XAttribute("Version", _version)
                    )
                );
        }

        #region IEnumerable(T) Methods
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
        #endregion
    }

}
