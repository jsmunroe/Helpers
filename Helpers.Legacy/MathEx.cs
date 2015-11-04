using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class MathEx
    {
        /// <summary>
        /// Get the maximum of the two given comparable values (<paramref name="a_first"/> and <paramref name="a_second"/>).
        /// </summary>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="a_first">First comparable value.</param>
        /// <param name="a_second">Second comparable value.</param>
        /// <returns>Maximum value or first value if equal.</returns>
        public static TValue Max<TValue>(TValue a_first, TValue a_second)
            where TValue : IComparable
        {
            if (ReferenceEquals(a_first, null) && ReferenceEquals(a_second, null))
                return default(TValue);

            if (ReferenceEquals(a_first, null))
                return a_second;

            if (ReferenceEquals(a_second, null))
                return a_first;

            if (a_first.CompareTo(a_second) > 0)
                return a_first;

            if (a_second.CompareTo(a_first) > 0)
                return a_second;

            return a_first;
        }

        /// <summary>
        /// Get the minimum of the two given comparable values (<paramref name="a_first"/> and <paramref name="a_second"/>).
        /// </summary>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="a_first">First comparable value.</param>
        /// <param name="a_second">Second comparable value.</param>
        /// <returns>Minimum value or first value if equal..</returns>
        public static TValue Min<TValue>(TValue a_first, TValue a_second)
            where TValue : IComparable
        {
            if (ReferenceEquals(a_first, null) && ReferenceEquals(a_second, null))
                return default(TValue);

            if (ReferenceEquals(a_first, null))
                return a_second;

            if (ReferenceEquals(a_second, null))
                return a_first;

            if (a_first.CompareTo(a_second) < 0)
                return a_first;

            if (a_second.CompareTo(a_first) < 0)
                return a_second;

            return a_first;
        }
    }
}
