using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Contracts;

namespace Helpers.IO
{
    public class FsDirectory : IDirectory
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_targetDirectory">Target directory.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_targetDirectory"/> is null.</exception>
        public FsDirectory(DirectoryInfo a_targetDirectory)
        {
            #region Argument Validation

            if (a_targetDirectory == null)
                throw new ArgumentNullException(nameof(a_targetDirectory));

            #endregion

            TargetDirectory = a_targetDirectory;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_directoryPath">Path to the target directory.</param>
        public FsDirectory(string a_directoryPath)
            : this(new DirectoryInfo(a_directoryPath))
        {

        }

        /// <summary>
        /// Target directory.
        /// </summary>
        public DirectoryInfo TargetDirectory { get; }

        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        public bool Exists => TargetDirectory.Exists;

        /// <summary>
        /// Directory name.
        /// </summary>
        public string Name => TargetDirectory.Name;

        /// <summary>
        /// Parent directory.
        /// </summary>
        public IDirectory Parent => TargetDirectory.Parent == null ? null : new FsDirectory(TargetDirectory.Parent);

        /// <summary>
        /// All subdirectories.
        /// </summary>
        public IEnumerable<IDirectory> Directories => TargetDirectory.EnumerateDirectories().Select(i => new FsDirectory(i));

        /// <summary>
        /// All files in this directory.
        /// </summary>
        public IEnumerable<IFile> Files => TargetDirectory.EnumerateFiles().Select(i => new FsFile(i));

        /// <summary>
        /// Get a direct child directory with the given name (<paramref name="a_name"/>).
        /// </summary>
        /// <param name="a_name">Child directory name.</param>
        /// <returns>Child directory.</returns>
        public IDirectory Directory(string a_name)
        {
            return new FsDirectory(TargetDirectory.Directory(a_name));
        }

        /// <summary>
        /// Get a file with the given name (<paramref name="a_name"/>) in this directory.
        /// </summary>
        /// <param name="a_name">File name.</param>
        /// <returns>File.</returns>
        public IFile File(string a_name)
        {
            return new FsFile(TargetDirectory.File(a_name));
        }

        /// <summary>
        /// Create the directory and every parent that is not already created.
        /// </summary>
        public void Create()
        {
            TargetDirectory.Create();
        }
    }

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
        /// Parent directory.
        /// </summary>
        public IDirectory Directory => new FsDirectory(TargetFile.Directory);
    }

}
