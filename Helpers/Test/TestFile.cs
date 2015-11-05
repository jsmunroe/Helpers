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
            Path = new PathBuilder(a_path);
            
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
                var parentPath = PathBuilder.Create(Path).Parent();
                if (parentPath == null)
                    return null;

                return new TestDirectory(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// Size of the file.
        /// </summary>
        public long Size => FileSystem.GetFileStats(Path).Size;

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc => FileSystem.GetFileStats(Path).CreatedTimeUtc;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc => FileSystem.GetFileStats(Path).LastModifiedTimeUtc;

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

            var fileStats = new TestFileStats();
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

            if (!Directory.Exists)
                throw new DirectoryNotFoundException("Cannot CopyTo because directory of source file does not exist");

            if (!Exists)
                throw new FileNotFoundException("Cannot CopyTo because source file does not exist.");

            var stats = new TestFileStats
            {
                Size = Size,
                CreatedTimeUtc = DateTime.UtcNow,
                LastModifiedTimeUtc = DateTime.UtcNow,
            };

            FileSystem.StageFile(a_dest.Path, stats);
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