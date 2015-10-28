using System;
using Helpers.Contracts;

namespace Helpers.Extensions
{
    public static class DirectoryHelpers
    {
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

            foreach (var file in a_this.Files)
            {
                var dest = a_destination.File(file.Name);
                file.CopyTo(dest);
            }

            foreach (var directory in a_this.Directories)
            {
                var dest = a_destination.Directory(directory.Name);
                directory.CopyTo(dest);
            }
        }

    }
}