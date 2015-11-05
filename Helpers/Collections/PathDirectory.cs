using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        public bool IsEmpty => Exists && !(Files().Any() || Directories().Any());

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
        /// Get all directories directly under this directory.
        /// </summary>
        /// <returns>All files directly under this directory.</returns>
        public IEnumerable<PathDirectory<TLeaf>> Directories()
        {
             return FileSystem.GetDirectories(Path).Select(i => new PathDirectory<TLeaf>(FileSystem, i)); 
        }

        /// <summary>
        /// Get all directories in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All directories in this directory matching the pattern.</returns>
        public IEnumerable<IDirectory> Directories(string a_pattern)
        {
            return FileSystem.GetDirectories(Path, a_pattern).Select(i => new PathDirectory<TLeaf>(FileSystem, i));
        }

        /// <summary>
        /// Get all files in this directory.
        /// </summary>
        /// <returns>All files in this directory.</returns>
        public IEnumerable<PathFile<TLeaf>> Files()
        {
             return FileSystem.GetFiles(Path).Select(i => new PathFile<TLeaf>(FileSystem, i)); 
        }


        /// <summary>
        /// Get all files in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All files in this directory matching the pattern.</returns>
        public IEnumerable<IFile> Files(string a_pattern)
        {
             return FileSystem.GetFiles(Path, a_pattern).Select(i => new PathFile<TLeaf>(FileSystem, i)); 
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
        /// Get the directory path for a file with the given name (<paramref name="a_name"/>) or relative path under this directory.
        /// </summary>
        /// <param name="a_name">Directory name or relative path.</param>
        /// <returns>Directory path.</returns>
        public PathBuilder DirectoryPath(string a_name)
        {
            return PathBuilder.Create(Path).Child(a_name);
        }

        /// <summary>
        /// Get the file path for a file with the given name (<paramref name="a_name"/>) or relative path under this directory.
        /// </summary>
        /// <param name="a_name">File name or relative path.</param>
        /// <returns>File path.</returns>
        public PathBuilder FilePath(string a_name)
        {
            return PathBuilder.Create(Path).Child(a_name);
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

            foreach (var file in Files())
                file.Delete();

            foreach (var directory in Directories())
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
        /// Get all directories directly under this directory.
        /// </summary>
        /// <returns>All files directly under this directory.</returns>
        IEnumerable<IDirectory> IDirectory.Directories()
        {
            return Directories();
        }

        /// <summary>
        /// Get all files in this directory.
        /// </summary>
        /// <returns>All files in this directory.</returns>
        IEnumerable<IFile> IDirectory.Files()
        {
            return Files();
        }

        /// <summary>
        /// Get all files in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All files in this directory matching the pattern.</returns>
        IEnumerable<IFile> IDirectory.Files(string a_pattern)
        {
            return Files(a_pattern);
        }

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        IDirectory IDirectory.Directory(string a_name)
        {
            return Directory(a_name);
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        IFile IDirectory.File(string a_name)
        {
            return File(a_name);
        }

        /// <summary>
        /// Refresh the state of this directory.
        /// </summary>
        void IDirectory.Refresh()
        {
            
        }

        #endregion
    }
}