using System;
using Helpers.Contracts;

namespace Helpers.Test
{
    public class TestFile : IFile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_path">File path.</param>
        public TestFile(string a_path)
            : this(new TestFileSystem(), a_path)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_fileSystem">Test file system.</param>
        /// <param name="a_path">File path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public TestFile(TestFileSystem a_fileSystem, string a_path)
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
        /// Whether the file exists.
        /// </summary>
        public bool Exists => FileSystem.FileExists(Path);

        /// <summary>
        /// File name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent directory.
        /// </summary>
        public IDirectory Directory
        {
            get
            {
                var parentPath = System.IO.Path.GetDirectoryName(Path);
                if (parentPath == null)
                    return null;

                return new TestDirectory(FileSystem, parentPath);
            }
        }
    }
}