using System;
using System.IO;
using Helpers.Contracts;

namespace Helpers.IO
{
    public class FsFile : IFile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_targetFile">Target file.</param>
        public FsFile(FileInfo a_targetFile)
        {
            TargetFile = a_targetFile;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_filePath">PathResult to the target file.</param>
        public FsFile(string a_filePath)
            : this(new FileInfo(a_filePath))
        {

        }

        /// <summary>
        /// Target file.
        /// </summary>
        public FileInfo TargetFile { get; }

        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        public bool Exists => TargetFile.Exists;

        /// <summary>
        /// Directory name.
        /// </summary>
        public string Name => TargetFile.Name;

        /// <summary>
        /// Full path to this directory.
        /// </summary>
        public string Path => TargetFile.FullName;

        /// <summary>
        /// Parent directory.
        /// </summary>
        public IDirectory Directory => new FsDirectory(TargetFile.Directory);

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc => TargetFile.CreationTimeUtc;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc => TargetFile.LastAccessTimeUtc;

        /// <summary>
        /// Size of the file.
        /// </summary>
        public long Size => TargetFile.Length;

        /// <summary>
        /// Create the file with the given stream (<paramref name="a_contents"/>) as its contents.
        /// </summary>
        /// <param name="a_contents">Stream contents.</param>
        public void Create(Stream a_contents)
        {
            using (var fout = TargetFile.Create())
                a_contents.CopyTo(fout);
        }

        /// <summary>
        /// Delete this file.
        /// </summary>
        public void Delete()
        {
            TargetFile.Delete();
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

            if (TargetFile.Exists)
                throw new FileNotFoundException("File cannot be copied because it does not exist.");

            TargetFile.CopyTo(a_dest.Path, true);
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

            var path = TargetFile.FullName;
            var newName = PathBuilder.Create(path).NameWithoutExtension() + "." + extension;
            var newPath = PathBuilder.Create(path).Parent().Child(newName);

            return new FsFile(newPath);
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