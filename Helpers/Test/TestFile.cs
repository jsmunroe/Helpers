using System;
using System.IO;
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

        /// <summary>
        /// Create the file with the given stream (<paramref name="a_contents"/>) as its contents.
        /// </summary>
        /// <param name="a_contents">Stream contents.</param>
        public void Create(Stream a_contents)
        {
            FileSystem.CreateFile(Path);
        }

        /// <summary>
        /// Delete this file.
        /// </summary>
        public void Delete()
        {
            FileSystem.DeleteFile(Path);
        }

        /// <summary>
        /// Create a file that is this file but with the given extension (<paramref name="a_extension"/>).
        /// </summary>
        /// <param name="a_extension">New extension.</param>
        /// <returns>Created file with the new extension.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_extension"/> is null.</exception>
        public IFile ChangeExtension(string a_extension)
        {
            #region Argument Validation

            if (a_extension == null)
                throw new ArgumentNullException(nameof(a_extension));

            #endregion

            var extension = a_extension.TrimStart('.');

            var path = Path;
            var newPath = System.IO.Path.GetDirectoryName(path) + "\\" +
                          System.IO.Path.GetFileNameWithoutExtension(path) + "." + extension;

            return new TestFile(FileSystem, newPath);
        }
    }
}