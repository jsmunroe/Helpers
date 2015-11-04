using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Helpers.Contracts;
using Helpers.IO;

namespace Helpers.Collections
{
    public class PathDirectory<TLeaf> : IDirectory
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        public PathDirectory(string a_path)
            : this(new PathTree<TLeaf>(), a_path)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_pathTree">Path tree.</param>
        /// <param name="a_path">Directory path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public PathDirectory(PathTree<TLeaf> a_pathTree, string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            FileSystem = a_pathTree ?? new PathTree<TLeaf>();
            Path = new PathBuilder(a_path);

            Name = PathBuilder.Create(Path).Name();
        }

        /// <summary>
        /// File system.
        /// </summary>
        public PathTree<TLeaf> FileSystem { get; }

        /// <summary>
        /// PathResult.
        /// </summary>
        public PathBuilder Path { get; }

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc { get; }

        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        public bool Exists => FileSystem.DirectoryExists(Path);

        /// <summary>
        /// Whether this directory is empty of subdirectories and files.
        /// </summary>
        public bool IsEmpty => Exists && !(Files.Any() || Directories.Any());

        /// <summary>
        /// Directory name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent directory.
        /// </summary>
        public IDirectory Parent
        {
            get
            {
                var parentPath = PathBuilder.Create(Path).Parent();
                if (parentPath == null)
                    return null;

                return new PathDirectory<TLeaf>(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// All subdirectories.
        /// </summary>
        public IEnumerable<PathDirectory<TLeaf>> Directories
        {
            get { return FileSystem.GetDirectories(Path).Select(i => new PathDirectory<TLeaf>(FileSystem, i)); }
        }

        /// <summary>
        /// All files in this directory.
        /// </summary>
        public IEnumerable<PathFile<TLeaf>> Files
        {
            get { return FileSystem.GetFiles(Path).Select(i => new PathFile<TLeaf>(FileSystem, i)); }
        }

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        public PathDirectory<TLeaf> Directory(string a_name)
        {
            var childPath = PathBuilder.Create(Path).Child(a_name);

            return new PathDirectory<TLeaf>(FileSystem, childPath);
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        public PathFile<TLeaf> File(string a_name)
        {
            var childPath = PathBuilder.Create(Path).Child(a_name);

            return new PathFile<TLeaf>(FileSystem, childPath);
        }

        /// <summary>
        /// Create the directory and every parent that is not already created.
        /// </summary>
        public void Create()
        {
            FileSystem.CreateDirectory(Path);
        }

        /// <summary>
        /// Delete this directory and every child under it.
        /// </summary>
        public void Delete()
        {
            if (!Exists)
                throw new DirectoryNotFoundException($"Directory at path \"{Path}\" does not exist.");

            FileSystem.DeleteDirectory(Path);
        }

        /// <summary>
        /// Delete every child under this directory and leave the directory itself alone.
        /// </summary>
        public void Empty()
        {
            if (!Exists)
                throw new DirectoryNotFoundException($"Directory at path \"{Path}\" does not exist.");

            foreach (var file in Files)
                file.Delete();

            foreach (var directory in Directories)
                directory.Delete();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Path;
        }

        #region IFileSystemBase Members

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        DateTime IFileSystemBase.CreatedTimeUtc => DateTime.UtcNow;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        DateTime IFileSystemBase.LastModifiedTimeUtc => DateTime.UtcNow;

        #endregion

        #region IDirectory 

        /// <summary>
        /// All subdirectories.
        /// </summary>
        IEnumerable<IDirectory> IDirectory.Directories
        {
            get { return FileSystem.GetDirectories(Path).Select(i => new PathDirectory<TLeaf>(FileSystem, i)); }
        }

        /// <summary>
        /// All files in this directory.
        /// </summary>
        IEnumerable<IFile> IDirectory.Files
        {
            get { return FileSystem.GetFiles(Path).Select(i => new PathFile<TLeaf>(FileSystem, i)); }
        }

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        IDirectory IDirectory.Directory(string a_name)
        {
            var childPath = PathBuilder.Create(Path).Child(a_name);

            return new PathDirectory<TLeaf>(FileSystem, childPath);
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        IFile IDirectory.File(string a_name)
        {
            var childPath = PathBuilder.Create(Path).Child(a_name);

            return new PathFile<TLeaf>(FileSystem, childPath);
        }

        #endregion
    }
}