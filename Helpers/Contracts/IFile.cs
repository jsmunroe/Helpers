using System;
using System.IO;

namespace Helpers.Contracts
{
    public interface IFile
    {
        /// <summary>
        /// Whether the file exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// File name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Full path to this file.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Parent directory.
        /// </summary>
        IDirectory Directory { get; }

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        DateTime CreatedTimeUtc { get; }

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        DateTime LastModifiedTimeUtc { get; }

        /// <summary>
        /// Size of the file.
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Create the file with the given stream (<paramref name="a_contents"/>) as its contents.
        /// </summary>
        /// <param name="a_contents">Stream contents.</param>
        void Create(Stream a_contents);

        /// <summary>
        /// Delete this file.
        /// </summary>
        void Delete();

        /// <summary>
        /// Copy from the given file (<paramref name="a_source"/>) to this file.
        /// </summary>
        /// <param name="a_source">File from which to copy.</param>
        void CopyTo(IFile a_source);

        /// <summary>
        /// Create a file that is this file but with the given extension (<paramref name="a_extension"/>).
        /// </summary>
        /// <param name="a_extension">New extension.</param>
        /// <returns>Created file with the new extension.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_extension"/> is null.</exception>
        IFile ChangeExtension(string a_extension);
    }
}