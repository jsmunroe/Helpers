using System;
using System.Collections.Generic;

namespace Helpers.Extensions
{
    public static class DictionaryHelpers
    {
        /// <summary>
        /// Get a value from "this" dictionary (<paramref name="a_this"/>) for the given key (<paramref name="a_key"/>) or the given default value (<paramref name="a_default"/>) it does not exist.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="a_this">"This" dictionary.</param>
        /// <param name="a_key">Key.</param>
        /// <param name="a_default">Default value.</param>
        /// <returns>Value of default value.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_key"/> is null.</exception>
        public static TValue GetValueWithDefault<TKey, TValue>(this IDictionary<TKey, TValue> a_this, TKey a_key, TValue a_default = default(TValue))
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException(nameof(a_this));

            if (a_key == null)
                throw new ArgumentNullException(nameof(a_key));

            #endregion

            if (a_this.ContainsKey(a_key))
                return a_this[a_key];

            return a_default;
        }
    }
}