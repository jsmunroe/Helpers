using System;
using System.IO;
using System.Text;

namespace Helpers.Test
{
    public class TestFileInstance
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestFileInstance()
        {
            CreatedTimeUtc = DateTime.UtcNow;
            LastModifiedTimeUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_data">Data string.</param>
        /// <param name="a_encoding">Encoding (default: UTF8).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_data"/> is null.</exception>
        public TestFileInstance(string a_data, Encoding a_encoding = null)
            : this()
        {
            #region Argument Validation

            if (a_data == null)
                throw new ArgumentNullException(nameof(a_data));

            #endregion

            a_encoding = a_encoding ?? Encoding.UTF8;

            Data = a_encoding.GetBytes(a_data);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_data">Data bytes.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_data"/> is null.</exception>
        public TestFileInstance(byte[] a_data)
            : this()
        {
            #region Argument Validation

            if (a_data == null)
                throw new ArgumentNullException(nameof(a_data));

            #endregion

            Data = a_data;
        }

        /// <summary>
        /// Size of the file.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Tag object.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// Data for this file.
        /// </summary>
        public byte[] Data { get; set; } = new byte[0];
    }
}