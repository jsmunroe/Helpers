﻿using System;
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
        /// <param name="a_filePath">Path to the target file.</param>
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
        /// Copy this file to the given file (<paramref name="a_source"/>).
        /// </summary>
        /// <param name="a_source">File to which to copy.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_source"/> is null.</exception>
        public void CopyTo(IFile a_source)
        {
            #region Argument Validation

            if (a_source == null)
                throw new ArgumentNullException(nameof(a_source));

            #endregion

            if (TargetFile.Exists)
                throw new FileNotFoundException("File cannot be copied because it does not exist.");

            TargetFile.CopyTo(a_source.Path);
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
            var newPath = System.IO.Path.GetDirectoryName(path) + "\\" +
                          System.IO.Path.GetFileNameWithoutExtension(path) + "." + extension;

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