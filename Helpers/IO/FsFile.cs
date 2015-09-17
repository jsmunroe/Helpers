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
    }
}