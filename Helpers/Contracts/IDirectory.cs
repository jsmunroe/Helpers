using System.Collections.Generic;
using Helpers.IO;

namespace Helpers.Contracts
{
    public interface IDirectory
    {
        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Directory name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parent directory.
        /// </summary>
        IDirectory Parent { get; }

        /// <summary>
        /// All subdirectories.
        /// </summary>
        IEnumerable<IDirectory> Directories { get; }

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        IDirectory Directory(string a_name);

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        IFile File(string a_name);

        /// <summary>
        /// Create the directory and every parent that is not already created.
        /// </summary>
        void Create();
    }
}