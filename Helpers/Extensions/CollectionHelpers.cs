using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Extensions
{
    public static class CollectionHelpers 
    {
        /// <summary>
        /// Add the given range of items (<paramref name="a_items"/>) to "this" collection (<paramref name="a_this"/>).
        /// </summary>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="a_this">"This" collection.</param>
        /// <param name="a_items">Sequence of items to add.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_items"/> is null.</exception>
        public static void AddRange<TItem>(this ICollection<TItem> a_this, IEnumerable<TItem> a_items)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException();

            if (a_items == null)
                throw new ArgumentNullException(nameof(a_items));

            #endregion

            foreach (var item in a_items)
                a_this.Add(item);
        }

        /// <summary>
        /// Add the given range of items (<paramref name="a_items"/>) to "this" collection (<paramref name="a_this"/>).
        /// </summary>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="a_this">"This" collection.</param>
        /// <param name="a_items">Sequence of items to add.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_items"/> is null.</exception>
        public static void AddRange<TItem>(this ICollection<TItem> a_this, params TItem[] a_items)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException();

            if (a_items == null)
                throw new ArgumentNullException(nameof(a_items));

            #endregion

            foreach (var item in a_items)
                a_this.Add(item);
        }

        /// <summary>
        /// Remove the given range of items (<paramref name="a_items"/>) to "this" collection (<paramref name="a_this"/>).
        /// </summary>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="a_this">"This" collection.</param>
        /// <param name="a_items">Sequence of items to remove.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_items"/> is null.</exception>
        public static void RemoveRange<TItem>(this ICollection<TItem> a_this, IEnumerable<TItem> a_items)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException();

            if (a_items == null)
                throw new ArgumentNullException(nameof(a_items));

            #endregion

            foreach (var item in a_items.ToArray())
                a_this.Remove(item);
        }

        /// <summary>
        /// Remove the given range of items (<paramref name="a_items"/>) to "this" collection (<paramref name="a_this"/>).
        /// </summary>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="a_this">"This" collection.</param>
        /// <param name="a_items">Sequence of items to remove.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_items"/> is null.</exception>
        public static void RemoveRange<TItem>(this ICollection<TItem> a_this, params TItem[] a_items)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException();

            if (a_items == null)
                throw new ArgumentNullException(nameof(a_items));

            #endregion

            foreach (var item in a_items.ToArray())
                a_this.Remove(item);
        }

        /// <summary>
        /// Remove any item in "this" collection (<paramref name="a_this"/>) for which the given predicate (<paramref name="a_predicate"/>) returns true.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="a_this">"This" collection.</param>
        /// <param name="a_predicate">Predicate.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_predicate"/> is null.</exception>
        public static void RemoveWhere<TItem>(this ICollection<TItem> a_this, Predicate<TItem> a_predicate)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException();

            if (a_predicate == null)
                throw new ArgumentNullException(nameof(a_predicate));

            #endregion

            foreach (var item in a_this.ToArray())
            {
                if (a_predicate(item))
                    a_this.Remove(item);
            }
        }
    }
}
