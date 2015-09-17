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
        /// Create the file with the given stream (<paramref name="a_contents"/>) as its contents.
        /// </summary>
        /// <param name="a_contents">Stream contents.</param>
        void Create(Stream a_contents);

        /// <summary>
        /// Delete this file.
        /// </summary>
        void Delete();
    }
}