using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Helpers.Contracts;

namespace Helpers.Files
{
    public class IniFile
    {
        private readonly IFile _source;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_source">Source file info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_source"/> is null.</exception>
        public IniFile(IFile a_source)
        {
            #region Argument Validation

            if (a_source == null)
                throw new ArgumentNullException(nameof(a_source));

            #endregion

            _source = a_source;
        }

        /// <summary>
        /// Whether the file exists.
        /// </summary>
        public bool Exists => _source.Exists;

        /// <summary>
        /// Get or set the value of the INI setting in the given section (<paramref name="a_section"/>) with the given key 
        ///     (<paramref name="a_key"/>) from this file.
        /// </summary>
        /// <param name="a_section">Setting section.</param>
        /// <param name="a_key">Setting key.</param>
        /// <returns>Setting value as text value.</returns>
        public TextValue this[string a_section, string a_key]
        {
            get { return new TextValue(ReadIniSetting(a_section, a_key)); }
            set { WriteIniSetting(a_section, a_key, (string)value); }
        }

        /// <summary>
        /// Write the given value (<paramref name="a_value"/>) as an INI setting in the given section 
        ///     (<paramref name="a_section"/>) with the given key (<paramref name="a_key"/>) to this file.
        /// </summary>
        /// <param name="a_section">Setting section.</param>
        /// <param name="a_key">Setting key.</param>
        /// <param name="a_value">New value of setting.</param>
        private void WriteIniSetting(string a_section, string a_key, string a_value)
        {
            if (!_source.Exists)
                _source.Create("");

            WritePrivateProfileString(a_section, a_key, a_value, _source.Path);
        }

        /// <summary>
        /// Read the current value of the INI setting in the given section (<paramref name="a_section"/>) with the given key 
        ///     (<paramref name="a_key"/>) from this file.
        /// </summary>
        /// <param name="a_section">Setting section.</param>
        /// <param name="a_key">Setting key.</param>
        /// <returns>Current value of setting.</returns>
        private string ReadIniSetting(string a_section, string a_key)
        {
            if (!_source.Exists)
                return "";

            var retVal = new StringBuilder(255);

            GetPrivateProfileString(a_section, a_key, "", retVal, 255, _source.Path);

            return retVal.ToString();
        }

        #region Kernel32 Interop

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        #endregion
    }
}
