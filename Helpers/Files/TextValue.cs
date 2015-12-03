using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class TextValue
    {
        private readonly string _value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_value">Value string.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_value"/> is null.</exception>
        public TextValue(string a_value)
        {
            #region Argument Validation

            if (a_value == null)
                throw new ArgumentNullException(nameof(a_value));

            #endregion

            _value = a_value;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return _value;
        }

        /// <summary>
        /// Cast the given text value (<paramref name="a_textValue"/>) into a string.
        /// </summary>
        /// <param name="a_textValue">Text value to cast.</param>
        /// <returns>String.</returns>
        public static explicit operator string(TextValue a_textValue)
        {
            return a_textValue._value;
        }

        /// <summary>
        /// Cast the given string value (<paramref name="a_value"/>) into a text value.
        /// </summary>
        /// <param name="a_value">String value to cast.</param>
        /// <returns>Text value.</returns>
        public static implicit operator TextValue(string a_value)
        {
            return new TextValue(a_value);
        }

        /// <summary>
        /// Cast the given text value (<paramref name="a_textValue"/>) into a string.
        /// </summary>
        /// <param name="a_textValue">Text value to cast.</param>
        /// <returns>String.</returns>
        public static explicit operator double(TextValue a_textValue)
        {
            double value;
            if (!double.TryParse(a_textValue._value, out value))
                return default(double);

            return value;
        }

        /// <summary>
        /// Cast the given string value (<paramref name="a_value"/>) into a text value.
        /// </summary>
        /// <param name="a_value">String value to cast.</param>
        /// <returns>Text value.</returns>
        public static implicit operator TextValue(double a_value)
        {
            return new TextValue(a_value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Cast the given text value (<paramref name="a_textValue"/>) into a string.
        /// </summary>
        /// <param name="a_textValue">Text value to cast.</param>
        /// <returns>String.</returns>
        public static explicit operator int(TextValue a_textValue)
        {
            int value;
            if (!int.TryParse(a_textValue._value, out value))
                return default(int);

            return value;
        }

        /// <summary>
        /// Cast the given string value (<paramref name="a_value"/>) into a text value.
        /// </summary>
        /// <param name="a_value">String value to cast.</param>
        /// <returns>Text value.</returns>
        public static implicit operator TextValue(int a_value)
        {
            return new TextValue(a_value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Cast the given text value (<paramref name="a_textValue"/>) into a string.
        /// </summary>
        /// <param name="a_textValue">Text value to cast.</param>
        /// <returns>String.</returns>
        public static explicit operator bool(TextValue a_textValue)
        {
            if (a_textValue._value.Equals("true", StringComparison.CurrentCultureIgnoreCase) ||
                a_textValue._value == "1" ||
                a_textValue._value.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// Cast the given string value (<paramref name="a_value"/>) into a text value.
        /// </summary>
        /// <param name="a_value">String value to cast.</param>
        /// <returns>Text value.</returns>
        public static implicit operator TextValue(bool a_value)
        {
            return new TextValue(a_value ? "true" : "false");
        }

    }
}
