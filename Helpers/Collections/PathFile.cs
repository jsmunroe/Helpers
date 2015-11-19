using System;
using System.IO;
using Helpers.Contracts;
using Helpers.IO;
using Helpers.Test;

namespace Helpers.Collections
{
    public class PathFile<TLeaf> : IFile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_pathTree">Path tree.</param>
        /// <param name="a_path">File path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public PathFile(PathTree<TLeaf> a_pathTree, string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            FileSystem = a_pathTree ?? new PathTree<TLeaf>();
            Path = new PathBuilder(a_path);

            Name = PathBuilder.Create(Path).Name();
        }

        /// <summary>
        /// File system.
        /// </summary>
        public PathTree<TLeaf> FileSystem { get; }

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
        public PathDirectory<TLeaf> Directory
        {
            get
            {
                var parentPath = PathBuilder.Create(Path).Parent();
                if (parentPath == null)
                    return null;

                return new PathDirectory<TLeaf>(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// Value of this file.
        /// </summary>
        public TLeaf Value => FileSystem.GetLeafValue(Path);

        
        /// <summary>
        /// Create the file to hold the given value (<paramref name="a_value"/>).
        /// </summary>
        /// <param name="a_value">Given value.</param>
        public void Create(TLeaf a_value)
        {
            FileSystem.CreateFile(Path, a_value);
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

            TLeaf value = default(TLeaf);
            var source = a_source as PathFile<TLeaf>;
            if (source != null)
                value = source.Value;

            FileSystem.CreateFile(Path, value);
        }

        /// <summary>
        /// Copy this file to the given file (<paramref name="a_dest"/>) using the given file copier (<paramref name="a_fileCopier"/>).
        /// </summary>
        /// <typeparam name="TDest">Type of destination file.</typeparam>
        /// <param name="a_dest">Destination file.</param>
        /// <param name="a_fileCopier">File copier.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_dest"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_fileCopier"/> is null.</exception>
        public void CopyTo<TDest>(TDest a_dest, IFileCopier<PathFile<TLeaf>, TDest> a_fileCopier)
            where TDest : IFile
        {
            #region Argument Validation

            if (a_dest == null)
                throw new ArgumentNullException(nameof(a_dest));

            if (a_fileCopier == null)
                throw new ArgumentNullException(nameof(a_fileCopier));

            #endregion

            a_fileCopier.Copy(this, a_dest);
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
            var newPath = PathBuilder.Create(Path).Sibling(newName);

            return new PathFile<TLeaf>(FileSystem, newPath);
        }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Path;
        }

        #region IFileSystemBase Members

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        DateTime IFileSystemBase.CreatedTimeUtc => DateTime.UtcNow;

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        DateTime IFileSystemBase.LastModifiedTimeUtc => DateTime.UtcNow;

        #endregion

        #region IFile Members

        /// <summary>
        /// Size of the file.
        /// </summary>
        long IFile.Size => 0;


        /// <summary>
        /// Create the file with the given stream (<paramref name="a_contents"/>) as its contents.
        /// </summary>
        /// <param name="a_contents">Stream contents.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_contents"/> is null.</exception>
        void IFile.Create(Stream a_contents)
        {

        }

        /// <summary>
        /// Parent directory.
        /// </summary>
        IDirectory IFile.Directory
        {
            get
            {
                var parentPath = Path.Parent();
                if (parentPath == null)
                    return null;

                return new PathDirectory<TLeaf>(FileSystem, parentPath);
            }
        }

        /// <summary>
        /// Refresh the state of this file.
        /// </summary>
        void IFile.Refresh()
        {

        }

        /// <summary>
        /// Get a readable stream for this file.
        /// </summary>
        /// <returns>Readable stream.</returns>
        Stream IFile.OpenRead()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a writable stream for this file.
        /// </summary>
        /// <returns></returns>
        Stream IFile.OpenWrite()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}