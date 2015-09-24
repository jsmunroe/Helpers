using System;
using System.IO;
using System.Text.RegularExpressions;
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

            FileStats = FileSystem.GetFileStats(Path) ?? 
                new TestFileStats
                {
                    Size = 0,
                    CreatedTimeUtc = DateTime.UtcNow,
                    LastModifiedTimeUtc = DateTime.UtcNow,
                };
        }

        /// <summary>
        /// File system.
        /// </summary>
        public TestFileSystem FileSystem { get; }
        
        /// <summary>
        /// File stats.
        /// </summary>
        public TestFileStats FileStats { get; }

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
        /// Size of the file.
        /// </summary>
        public long Size => FileStats.Size;

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc => FileStats.CreatedTimeUtc;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc => FileStats.LastModifiedTimeUtc;

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

            FileStats.Size = a_contents.Length;
            FileStats.CreatedTimeUtc = DateTime.UtcNow;
            FileStats.LastModifiedTimeUtc = DateTime.UtcNow;

            FileSystem.CreateFile(Path, FileStats);
        }

        /// <summary>
        /// Delete this file.
        /// </summary>
        public void Delete()
        {
            FileSystem.DeleteFile(Path);
        }

        /// <summary>
        /// Copy from the given file (<paramref name="a_source"/>) to this file.
        /// </summary>
        /// <param name="a_source">File from which to copy.</param>
        public void CopyTo(IFile a_source)
        {
            var stats = new TestFileStats
            {
                Size = a_source.Size,
                CreatedTimeUtc = DateTime.UtcNow,
                LastModifiedTimeUtc = DateTime.UtcNow,
            };

            FileSystem.CreateFile(a_source.Path, stats);
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

            var newPath = System.IO.Path.GetDirectoryName(Path) + "\\" +
                          System.IO.Path.GetFileNameWithoutExtension(Path) + "." + extension;

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
    }
}