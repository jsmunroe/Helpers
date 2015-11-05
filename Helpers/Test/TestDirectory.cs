using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Contracts;
using Helpers.IO;

namespace Helpers.Test
{
    public class TestDirectory : IDirectory
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        public TestDirectory(string a_path)
            : this(new TestFileSystem(), a_path)
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_fileSystem">Test file system.</param>
        /// <param name="a_path">Directory path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public TestDirectory(TestFileSystem a_fileSystem, string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            FileSystem = a_fileSystem ?? new TestFileSystem();
            Path = new PathBuilder(a_path);

            Name = PathBuilder.Create(Path).Name();
        }

        /// <summary>
        /// File system.
        /// </summary>
        public TestFileSystem FileSystem { get; }

        /// <summary>
        /// Directory name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// PathResult.
        /// </summary>
        public PathBuilder Path { get; }

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc { get; } = new DateTime();

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; } = new DateTime();

        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        public bool Exists => FileSystem.DirectoryExists(Path);

        /// <summary>
        /// Whether this directory is empty of subdirectories and files.
        /// </summary>
        public bool IsEmpty => Exists && !(Files().Any() || Directories().Any());


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

                return new TestDirectory(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// Get all directories directly under this directory.
        /// </summary>
        /// <returns>All files directly under this directory.</returns>
        public IEnumerable<IDirectory> Directories()
        {
            return FileSystem.GetDirectories(Path).Select(i => new TestDirectory(FileSystem, i));
        }

        /// <summary>
        /// Get all directories in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All directories in this directory matching the pattern.</returns>
        public IEnumerable<IDirectory> Directories(string a_pattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all files in this directory.
        /// </summary>
        /// <returns>All files in this directory.</returns>
        public IEnumerable<IFile> Files()
        {
            return FileSystem.GetFiles(Path).Select(i => new TestFile(FileSystem, i));
        }

        /// <summary>
        /// Get all files in this directory matching the given pattern (<paramref name="a_pattern"/>).
        /// </summary>
        /// <param name="a_pattern">File match pattern.</param>
        /// <returns>All files in this directory matching the pattern.</returns>
        public IEnumerable<IFile> Files(string a_pattern)
        {
            return FileSystem.GetFiles(Path, a_pattern).Select(i => new TestFile(FileSystem, i));
        }


        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_name"/> is null.</exception>
        public IDirectory Directory(string a_name)
        {
            #region Argument Validation

            if (a_name == null)
                throw new ArgumentNullException(nameof(a_name));

            #endregion

            return new TestDirectory(FileSystem, DirectoryPath(a_name));
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_name"/> is null.</exception>
        public IFile File(string a_name)
        {
            #region Argument Validation

            if (a_name == null)
                throw new ArgumentNullException(nameof(a_name));

            #endregion

            return new TestFile(FileSystem, FilePath(a_name));
        }

        /// <summary>
        /// Get the directory path for a file with the given name (<paramref name="a_name"/>) or relative path under this directory.
        /// </summary>
        /// <param name="a_name">Directory name or relative path.</param>
        /// <returns>Directory path.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_name"/> is null.</exception>
        public PathBuilder DirectoryPath(string a_name)
        {
            #region Argument Validation

            if (a_name == null)
                throw new ArgumentNullException(nameof(a_name));

            #endregion

            return PathBuilder.Create(Path).Child(a_name);
        }

        /// <summary>
        /// Get the file path for a file with the given name (<paramref name="a_name"/>) or relative path under this directory.
        /// </summary>
        /// <param name="a_name">File name or relative path.</param>
        /// <returns>File path.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_name"/> is null.</exception>
        public PathBuilder FilePath(string a_name)
        {
            #region Argument Validation

            if (a_name == null)
                throw new ArgumentNullException(nameof(a_name));

            #endregion

            return PathBuilder.Create(Path).Child(a_name);
        }

        /// <summary>
        /// Create the directory and every parent that is not already created.
        /// </summary>
        public void Create()
        {
            FileSystem.StageDirectory(Path);
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

        #region IDirectory 

        /// <summary>
        /// Refresh the state of this directory.
        /// </summary>
        void IDirectory.Refresh()
        {

        }

        #endregion

    }
}
