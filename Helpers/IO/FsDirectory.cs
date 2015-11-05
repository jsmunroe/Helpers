using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Contracts;

namespace Helpers.IO
{
    public class FsDirectory : IDirectory
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_targetDirectory">Target directory.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_targetDirectory"/> is null.</exception>
        public FsDirectory(DirectoryInfo a_targetDirectory)
        {
            #region Argument Validation

            if (a_targetDirectory == null)
                throw new ArgumentNullException(nameof(a_targetDirectory));

            #endregion

            TargetDirectory = a_targetDirectory;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_directoryPath">PathResult to the target directory.</param>
        public FsDirectory(string a_directoryPath)
            : this(new DirectoryInfo(a_directoryPath))
        {

        }

        /// <summary>
        /// Target directory.
        /// </summary>
        public DirectoryInfo TargetDirectory { get; }

        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        public bool Exists => TargetDirectory.Exists;

        /// <summary>
        /// Whether this directory is empty of subdirectories and files.
        /// </summary>
        public bool IsEmpty => Exists && !(Files().Any() || Directories().Any());

        /// <summary>
        /// Directory name.
        /// </summary>
        public string Name => TargetDirectory.Name;

        /// <summary>
        /// Full path to this directory.
        /// </summary>
        public PathBuilder Path => new PathBuilder(TargetDirectory.FullName);

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc => TargetDirectory.CreationTimeUtc;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc => TargetDirectory.LastAccessTimeUtc;

        /// <summary>
        /// Parent directory.
        /// </summary>
        public IDirectory Parent => TargetDirectory.Parent == null ? null : new FsDirectory(TargetDirectory.Parent);

        /// <summary>
        /// Get all directories directly under this directory.
        /// </summary>
        /// <returns>All files directly under this directory.</returns>
        public IEnumerable<IDirectory> Directories()
        {
            return TargetDirectory.EnumerateDirectories().Select(i => new FsDirectory(i));
        }

        /// <summary>
        /// Get all directories in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All directories in this directory matching the pattern.</returns>
        public IEnumerable<IDirectory> Directories(string a_pattern)
        {
            return TargetDirectory.EnumerateDirectories(a_pattern).Select(i => new FsDirectory(i));
        }

        /// <summary>
        /// Get all files in this directory.
        /// </summary>
        /// <returns>All files in this directory.</returns>
        public IEnumerable<IFile> Files()
        {
            return TargetDirectory.EnumerateFiles().Select(i => new FsFile(i));
        }

        /// <summary>
        /// Get all files in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All files in this directory matching the pattern.</returns>
        public IEnumerable<IFile> Files(string a_pattern)
        {
            return TargetDirectory.EnumerateFiles(a_pattern).Select(i => new FsFile(i));
        }

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        public IDirectory Directory(string a_name)
        {
            return new FsDirectory(TargetDirectory.Directory(a_name));
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        public IFile File(string a_name)
        {
            return new FsFile(TargetDirectory.File(a_name));
        }

        /// <summary>
        /// Get the directory path for a file with the given name (<paramref name="a_name"/>) or relative path under this directory.
        /// </summary>
        /// <param name="a_name">Directory name or relative path.</param>
        /// <returns>Directory path.</returns>
        public PathBuilder DirectoryPath(string a_name)
        {
            return PathBuilder.Create(TargetDirectory.DirectoryPath(a_name));
        }

        /// <summary>
        /// Get the file path for a file with the given name (<paramref name="a_name"/>) or relative path under this directory.
        /// </summary>
        /// <param name="a_name">File name or relative path.</param>
        /// <returns>File path.</returns>
        public PathBuilder FilePath(string a_name)
        {
            return PathBuilder.Create(TargetDirectory.FilePath(a_name));
        }

        /// <summary>
        /// Create the directory and every parent that is not already created.
        /// </summary>
        public void Create()
        {
            TargetDirectory.Create();
        }

        /// <summary>
        /// Delete this directory and every child under it.
        /// </summary>
        public void Delete()
        {
            TargetDirectory.Delete(true);
        }

        /// <summary>
        /// Delete every child under this directory and leave the directory itself alone.
        /// </summary>
        public void Empty()
        {
            foreach (var file in TargetDirectory.GetFiles())
                file.Delete();

            foreach (var directory in TargetDirectory.GetDirectories())
                directory.Delete(true);
        }

        /// <summary>
        /// Refresh the state of this directory.
        /// </summary>
        public void Refresh()
        {
            TargetDirectory.Refresh();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Path;
        }
    }
}
