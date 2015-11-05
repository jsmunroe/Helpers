using System.Collections.Generic;

namespace Helpers.Contracts
{
    public interface IDirectory : IFileSystemBase
    {
        /// <summary>
        /// Whether this directory is empty of subdirectories and files.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Parent directory.
        /// </summary>
        IDirectory Parent { get; }

        /// <summary>
        /// Get all directories directly under this directory.
        /// </summary>
        /// <returns>All files directly under this directory.</returns>
        IEnumerable<IDirectory> Directories();

        /// <summary>
        /// Get all files in this directory.
        /// </summary>
        /// <returns>All files in this directory.</returns>
        IEnumerable<IFile> Files();

        /// <summary>
        /// Get all files in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All files in this directory matching the pattern.</returns>
        IEnumerable<IFile> Files(string a_pattern);

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

        /// <summary>
        /// Delete this directory and every child under it.
        /// </summary>
        void Delete();

        /// <summary>
        /// Delete every child under this directory and leave the directory itself alone.
        /// </summary>
        void Empty();
    }
}