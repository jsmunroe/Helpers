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
        public PathBuilder Path => new PathBuilder(TargetFile.FullName);

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

            if (!a_source.Exists)
                throw new FileNotFoundException("File cannot be copied because it does not exist.");

            var source = a_source as FsFile;
            if (source == null)
                throw new InvalidOperationException($"Cannot copy from a source of type\"{a_source.GetType().Name}\".");

            File.Copy(a_source.Path, Path, true);
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
        /// Refresh the state of this file.
        /// </summary>
        public void Refresh()
        {
            TargetFile.Refresh();
        }

        /// <summary>
        /// Open a readable stream for this file.
        /// </summary>
        /// <returns>Readable stream.</returns>
        public Stream OpenRead()
        {
            return TargetFile.OpenRead();
        }

        /// <summary>
        /// Open a writable stream for this file.
        /// </summary>
        /// <returns></returns>
        public Stream OpenWrite()
        {
            return TargetFile.OpenWrite();
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