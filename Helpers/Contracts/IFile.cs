using System;
using System.IO;
using Helpers.IO;

namespace Helpers.Contracts
{
    public interface IFile : IFileSystemBase
    {
        /// <summary>
        /// Parent directory.
        /// </summary>
        IDirectory Directory { get; }

        /// <summary>
        /// Size of the file.
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Create a file with the given text (<paramref name="a_text"/>) as its contents.
        /// </summary>
        /// <param name="a_text">Text contents.</param>
        void Create(string a_text);

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
        /// Copy this file to the given file (<paramref name="a_dest"/>) overwriting if necessary.
        /// </summary>
        /// <param name="a_dest">File to which to copy.</param>
        void CopyTo(IFile a_dest);

        /// <summary>
        /// Copy from the given file (<paramref name="a_source"/> to this one overwriting if necessary..
        /// </summary>
        /// <param name="a_source">File from which to copy.</param>
        void CopyFrom(IFile a_source);

        /// <summary>
        /// Create a file that is this file but with the given extension (<paramref name="a_extension"/>).
        /// </summary>
        /// <param name="a_extension">New extension.</param>
        /// <returns>Created file with the new extension.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_extension"/> is null.</exception>
        IFile ChangeExtension(string a_extension);

        /// <summary>
        /// Refresh the state of this file.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Get a readable stream for this file.
        /// </summary>
        /// <returns>Readable stream.</returns>
        Stream OpenRead();

        /// <summary>
        /// Get a writable stream for this file.
        /// </summary>
        /// <returns></returns>
        Stream OpenWrite();
    }
}