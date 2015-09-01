using System;
using System.IO;

namespace Helpers.IO
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Get a child file under the given directory (<paramref name="a_directory"/>) with the given name (<paramref name="a_fileName"/>).
        /// </summary>
        /// <param name="a_directory">"This" directory.</param>
        /// <param name="a_fileName">File name.</param>
        /// <returns>File info of the child file.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_directory"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_fileName"/> is null.</exception>
        public static FileInfo File(this DirectoryInfo a_directory, string a_fileName)
        {
            #region Argument Validation

            if (a_directory == null)
                throw new NullReferenceException(nameof(a_directory));

            if (a_fileName == null)
                throw new ArgumentNullException(nameof(a_fileName));

            #endregion

            var filePath = FilePath(a_directory, a_fileName);

            return new FileInfo(filePath);
        }

        /// <summary>
        /// Get a child file under the given directory (<paramref name="a_directory"/>) with the given name (<paramref name="a_fileName"/>).
        /// </summary>
        /// <param name="a_directory">"This" directory.</param>
        /// <param name="a_fileName">File name.</param>
        /// <returns>File path to the child file.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_directory"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_fileName"/> is null.</exception>
        public static string FilePath(this DirectoryInfo a_directory, string a_fileName)
        {
            #region Argument Validation

            if (a_directory == null)
                throw new NullReferenceException(nameof(a_directory));

            if (a_fileName == null)
                throw new ArgumentNullException(nameof(a_fileName));

            #endregion

            return Path.Combine(a_directory.FullName, a_fileName);
        }

        /// <summary>
        /// Get a child directory under the given directory (<paramref name="a_directory"/>) with the given name (<paramref name="a_fileName"/>).
        /// </summary>
        /// <param name="a_directory">"This" directory.</param>
        /// <param name="a_directoryName">Directory name.</param>
        /// <returns>Directory info of the child directory.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_directory"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_directoryName"/> is null.</exception>
        public static DirectoryInfo Directory(this DirectoryInfo a_directory, string a_directoryName)
        {
            #region Argument Validation

            if (a_directory == null)
                throw new NullReferenceException(nameof(a_directory));

            if (a_directoryName == null)
                throw new ArgumentNullException(nameof(a_directoryName));

            #endregion

            var directoryPath = DirectoryPath(a_directory, a_directoryName);

            return new DirectoryInfo(directoryPath);
        }

        /// <summary>
        /// Get a child directory under the given directory (<paramref name="a_directory"/>) with the given name (<paramref name="a_fileName"/>).
        /// </summary>
        /// <param name="a_directory">"This" directory.</param>
        /// <param name="a_directoryName">Directory name.</param>
        /// <returns>Directory path to the child directory.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_directory"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_directoryName"/> is null.</exception>
        public static string DirectoryPath(this DirectoryInfo a_directory, string a_directoryName)
        {
            #region Argument Validation

            if (a_directory == null)
                throw new NullReferenceException(nameof(a_directory));

            if (a_directoryName == null)
                throw new ArgumentNullException(nameof(a_directoryName));

            #endregion

            return Path.Combine(a_directory.FullName, a_directoryName);
        }
    }
}
