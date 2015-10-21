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
            Path = a_path;

            Name = PathBuilder.Create(Path).Name();
        }

        /// <summary>
        /// File system.
        /// </summary>
        public TestFileSystem FileSystem { get; }

        /// <summary>
        /// PathResult.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        public bool Exists => FileSystem.DirectoryExists(Path);

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

                return new TestDirectory(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// All subdirectories.
        /// </summary>
        public IEnumerable<IDirectory> Directories
        {
            get { return FileSystem.GetDirectories(Path).Select(i => new TestDirectory(FileSystem, i)); }
        }

        /// <summary>
        /// All files in this directory.
        /// </summary>
        public IEnumerable<IFile> Files
        {
            get { return FileSystem.GetFiles(Path).Select(i => new TestFile(FileSystem, i)); }
        }

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        public IDirectory Directory(string a_name)
        {
            var childPath = PathBuilder.Create(Path).Child(a_name);

            return new TestDirectory(FileSystem, childPath);
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        public IFile File(string a_name)
        {
            var childPath = PathBuilder.Create(Path).Child(a_name);

            return new TestFile(FileSystem, childPath);
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
    }
}
