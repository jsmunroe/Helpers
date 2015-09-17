using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Contracts;

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

            Name = System.IO.Path.GetFileName(Path);              
        }

        /// <summary>
        /// File system.
        /// </summary>
        public TestFileSystem FileSystem { get; }

        /// <summary>
        /// Path.
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
                var parentPath = System.IO.Path.GetDirectoryName(Path);
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
            var childPath = System.IO.Path.Combine(Path, a_name);

            return new TestDirectory(FileSystem, childPath);
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        public IFile File(string a_name)
        {
            var childPath = System.IO.Path.Combine(Path, a_name);

            return new TestFile(FileSystem, childPath);
        }

        /// <summary>
        /// Create the directory and every parent that is not already created.
        /// </summary>
        public void Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete this directory and every child under it.
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
