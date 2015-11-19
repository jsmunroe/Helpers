using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Helpers.Contracts;

namespace Helpers.Extensions
{
    public static class DirectoryHelpers
    {
        /// <summary>
        /// Enumerate through all descendent directories under "this" directory (<paramref name="a_this"/>).
        /// </summary>
        /// <param name="a_this">"This" directory.</param>
        /// <returns>All descendent directories.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        public static IEnumerable<IDirectory> EnumerateDescendentDirectories(this IDirectory a_this)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException(nameof(a_this));

            #endregion

            foreach (var directory in a_this.Directories())
            {
                yield return directory;

                foreach (var descendent in EnumerateDescendentDirectories(directory))
                    yield return descendent;
            }
        }

        /// <summary>
        /// Enumerate through all descendent files under "this" directory (<paramref name="a_this"/>).
        /// </summary>
        /// <param name="a_this">"This" directory.</param>
        /// <returns>All descendent files.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        public static IEnumerable<IFile> EnumerateDescendentFiles(this IDirectory a_this)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException(nameof(a_this));

            #endregion

            foreach (var file in a_this.Files())
                yield return file;

            foreach (var directory in a_this.Directories())
            {
                foreach (var file in EnumerateDescendentFiles(directory))
                    yield return file;
            }
        }

        /// <summary>
        /// Get all descendent directories under "this" directory (<paramref name="a_this"/>).
        /// </summary>
        /// <param name="a_this">"This" directory.</param>
        /// <returns>All descendent directories.</returns>
        public static IDirectory[] GetDescendentDirectories(this IDirectory a_this)
        {
            return EnumerateDescendentDirectories(a_this).ToArray();
        }

        /// <summary>
        /// Get all descendent files under "this" directory (<paramref name="a_this"/>).
        /// </summary>
        /// <param name="a_this">"This" directory.</param>
        /// <returns>All descendent files.</returns>
        public static IFile[] GetDescendentFiles(this IDirectory a_this)
        {
            return EnumerateDescendentFiles(a_this).ToArray();
        }

        /// <summary>
        /// Copy "this" directory (<paramref name="a_this"/>) to the given destination directory (<paramref name="a_destination"/>).
        /// </summary>
        /// <param name="a_this">"This" directory.</param>
        /// <param name="a_destination">Destination directory.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_destination"/> is null.</exception>
        public static void CopyTo(this IDirectory a_this, IDirectory a_destination)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException(nameof(a_this));

            if (a_destination == null)
                throw new ArgumentNullException(nameof(a_destination));

            #endregion

            a_destination.Create();

            foreach (var file in a_this.Files())
            {
                var dest = a_destination.File(file.Name);
                file.CopyTo(dest);
            }

            foreach (var directory in a_this.Directories())
            {
                var dest = a_destination.Directory(directory.Name);
                directory.CopyTo(dest);
            }
        }

        /// <summary>
        /// Copy the given directory (<paramref name="a_directory"/>) into "this" directory (<paramref name="a_this"/>).
        /// </summary>
        /// <param name="a_this">"This" directory.</param>
        /// <param name="a_directory">Directory to copy.</param>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="a_this"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_directory"/> is null.</exception>
        public static IDirectory CopyIn(this IDirectory a_this, IDirectory a_directory)
        {
            #region Argument Validation

            if (a_this == null)
                throw new NullReferenceException(nameof(a_this));

            if (a_directory == null)
                throw new ArgumentNullException(nameof(a_directory));

            #endregion

            if (!a_directory.Exists)
                throw new DirectoryNotFoundException($"Directory at path \"{a_directory.Path}\" does not exist.");

            if (!a_this.Exists)
                throw new DirectoryNotFoundException($"Directory at path \"{a_this.Path}\" does not exist.");

            var newDirectory = a_this.Directory(a_directory.Name);
            a_directory.CopyTo(newDirectory);

            return newDirectory;
        }


    }
}