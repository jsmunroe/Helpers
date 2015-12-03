using System;
using System.IO;
using System.Text.RegularExpressions;
using Helpers.Contracts;
using Helpers.IO;

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
            Path = new PathBuilder(a_path).WithRoot(PathBuilder.WindowsDriveRoot);

            Name = PathBuilder.Create(Path).Name();
        }

        /// <summary>
        /// File system.
        /// </summary>
        public TestFileSystem FileSystem { get; }

        /// <summary>
        /// PathResult.
        /// </summary>
        public PathBuilder Path { get; }

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
                var parentPath = Path.Parent();
                if (parentPath == null)
                    return null;

                return new TestDirectory(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// Size of the file.
        /// </summary>
        public long Size => Instance.Size;

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc => Instance.CreatedTimeUtc;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc => Instance.LastModifiedTimeUtc;

        /// <summary>
        /// Get the test file instance for this item.
        /// </summary>
        public TestFileInstance Instance => FileSystem.GetFileInstance(Path);

        /// <summary>
        /// Create a file with the given text (<paramref name="a_text"/>) as its contents.
        /// </summary>
        /// <param name="a_text">Text contents.</param>
        public void Create(string a_text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create the file with the given stream (<paramref name="a_contents"/>) as its contents.
        /// </summary>
        /// <param name="a_contents">Stream contents.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_contents"/> is null.</exception>
        public void Create(Stream a_contents)
        {
            #region Argument Validation

            if (a_contents == null)
                throw new ArgumentNullException(nameof(a_contents));

            #endregion

            var fileStats = new TestFileInstance();
            fileStats.Size = a_contents.Length;
            fileStats.CreatedTimeUtc = DateTime.UtcNow;
            fileStats.LastModifiedTimeUtc = DateTime.UtcNow;

            FileSystem.StageFile(Path, fileStats);
        }

        /// <summary>
        /// Delete this file.
        /// </summary>
        public void Delete()
        {
            if (!Directory.Exists)
                throw new DirectoryNotFoundException("Cannot CopyTo because directory of source file does not exist");

            FileSystem.DeleteFile(Path);
        }

        /// <summary>
        /// Copy this file to the given file (<paramref name="a_dest"/>).
        /// </summary>
        /// <param name="a_dest">File to which to copy.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_dest"/> is null.</exception>
        public void CopyTo(IFile a_dest)
        {
            #region Argument Validation

            if (a_dest == null)
                throw new ArgumentNullException(nameof(a_dest));

            #endregion

            a_dest.CopyFrom(this); // This allows "CopyTo" to work in betweeen file system types.
        }

        /// <summary>
        /// Copy from the given file (<paramref name="a_source"/> to this one overwriting if necessary..
        /// </summary>
        /// <param name="a_source">File from which to copy.</param>
        public void CopyFrom(IFile a_source)
        {
            #region Argument Validation

            if (a_source == null)
                throw new ArgumentNullException(nameof(a_source));

            #endregion

            if (!a_source.Directory.Exists)
                throw new DirectoryNotFoundException("Cannot CopyTo because directory of source file does not exist");

            if (!a_source.Exists)
                throw new FileNotFoundException("Cannot CopyTo because source file does not exist.");

            TestFileInstance instance = null;

            var source = a_source as TestFile;
            if (source != null)
                instance = source.Instance.Clone();

            FileSystem.StageFile(Path, instance);
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

            var newName = PathBuilder.Create(Path).NameWithoutExtension() + "." + extension;
            var newPath = PathBuilder.Create(Path).Parent().Child(newName);

            return new TestFile(FileSystem, newPath);
        }


        /// <summary>
        /// Get a readable stream for this file.
        /// </summary>
        /// <returns>Readable stream.</returns>
        public Stream OpenRead()
        {
            return FileSystem.OpenRead(Path);
        }

        /// <summary>
        /// Get a writable stream for this file.
        /// </summary>
        /// <returns>Writable stream.</returns>
        public Stream OpenWrite()
        {
            return FileSystem.OpenWrite(Path);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Path;
        }

        #region IFile Members 

        /// <summary>
        /// Refresh the state of this file.
        /// </summary>
        void IFile.Refresh()
        {
            // Not used.
        }

        #endregion
    }
}