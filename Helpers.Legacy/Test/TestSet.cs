using System;
using System.Collections;
using System.Collections.Generic;
using Helpers.Contracts;

namespace Helpers.Test
{
    public class TestSet<TEntity> : IFileSet<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestSet()
        {
            Entities = new List<TEntity>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_items">Initialize items.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_items"/> is null.</exception>
        public TestSet(params TEntity[] a_items)
        {
            #region Argument Validation

            if (a_items == null)
                throw new ArgumentNullException(nameof(a_items));

            #endregion

            Entities = new List<TEntity>(a_items);
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
        protected List<TEntity> Entities { get; private set; }

        /// <summary>
        /// Add the given entity (<paramref name="a_item"/>) to this set.
        /// </summary>
        /// <param name="a_item">Element to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_item"/> is null.</exception>
        public void Add(TEntity a_item)
        {
            #region Argument Validation

            if (a_item == null)
                throw new ArgumentNullException(nameof(a_item));

            #endregion

            Entities.Add(a_item);
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

        }

        /// <summary>
        /// Revert the set back to its file.
        /// </summary>
        public void Revert()
        {
            
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