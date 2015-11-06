using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Helpers.Contracts;
using Helpers.IO;

namespace Helpers.Test
{
    public class TestFileSystem
    {
        private readonly Dictionary<string, Dictionary<string, TestFileInstance>> _directories = new Dictionary<string, Dictionary<string, TestFileInstance>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Create the directory with the given path (<paramref name="a_path"/>) within this file system.
        /// </summary>
        /// <param name="a_path">Directory to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <returns>Created test directory.</returns>
        public TestDirectory StageDirectory(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!_directories.ContainsKey(a_path))
            {
                _directories.Add(a_path, new Dictionary<string, TestFileInstance>(StringComparer.OrdinalIgnoreCase));

                var parent = PathBuilder.Create(a_path).WithRoot(PathBuilder.WindowsDriveRoot).Parent();
                if (parent != null)
                    StageDirectory(parent);
            }

            return new TestDirectory(this, a_path);
        }

        /// <summary>
        /// Get whether the directory with given path (<paramref name="a_path"/>) exists in this file system.
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <returns>True iff the directory exists.</returns>
        public bool DirectoryExists(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            return _directories.ContainsKey(a_path);
        }

        /// <summary>
        /// Delete the directory at the given path (<paramref name="a_path"/>) and all of its descendtent files and directories.
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public void DeleteDirectory(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var toDelete = _directories.Keys.Where(i => i.StartsWith(a_path, StringComparison.OrdinalIgnoreCase)).ToArray();

            foreach (var key in toDelete)
                _directories.Remove(key);
        }

        /// <summary>
        /// Create a file with the given path (<paramref name="a_path"/>) within this file system.
        /// </summary>
        /// <param name="a_path">File path.</param>
        /// <param name="a_file">File stats.</param>
        /// <returns>This file system used with fluent interface.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_file"/> is null.</exception>
        public TestFile StageFile(string a_path, TestFileInstance a_file = null)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var pb = PathBuilder.Create(a_path).WithRoot(PathBuilder.WindowsDriveRoot);
            var directory = pb.Parent();
            var file = pb.Name();

            StageDirectory(directory);

            var files = _directories[directory];
            files[file] = a_file ?? new TestFileInstance();

            return new TestFile(this, a_path);
        }

        /// <summary>
        /// Get whether the file with given path (<paramref name="a_path"/>) exists in this file system.
        /// </summary>
        /// <param name="a_path">File path.</param>
        /// <returns>True iff the file exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public bool FileExists(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var pb = PathBuilder.Create(a_path).WithRoot(PathBuilder.WindowsDriveRoot);
            var directory = pb.Parent();
            var file = pb.Name();


            if (!DirectoryExists(directory))
                return false;

            var files = _directories[directory];
            return files.ContainsKey(file);
        }

        /// <summary>
        /// Delete the file at the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">File path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public void DeleteFile(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var pb = PathBuilder.Create(a_path).WithRoot(PathBuilder.WindowsDriveRoot);
            var directory = pb.Parent();
            var file = pb.Name();

            if (!DirectoryExists(directory))
                return;

            var files = _directories[directory];
            files.Remove(file);
        }

        /// <summary>
        /// Get the paths of the files in the directory with the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <returns>File paths.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public PathBuilder[] GetFiles(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!DirectoryExists(a_path))
                throw new DirectoryNotFoundException($"Directory at path \"{a_path}\" does not exist.");

            var files = _directories[a_path].Keys.Select(i => PathBuilder.Create(a_path).Child(i));

            return files.ToArray();
        }

        /// <summary>
        /// Get the paths of the files in the directory with the given path (<paramref name="a_path"/>) that match the 
        ///     given search pattern (<paramref name="a_searchPattern"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <param name="a_searchPattern">Search pattern.</param>
        /// <returns>File paths.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_searchPattern"/> is null.</exception>
        public PathBuilder[] GetFiles(string a_path, string a_searchPattern)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            if (a_searchPattern == null)
                throw new ArgumentNullException(nameof(a_searchPattern));

            #endregion

            var searchPattern = "^" + Regex.Escape(a_searchPattern).Replace("\\*", ".*") + "$";
            var rexSearch = new Regex(searchPattern, RegexOptions.IgnoreCase);

            var files = GetFiles(a_path).Where(i => rexSearch.IsMatch(i.Name()));

            return files.ToArray();
        }

        /// <summary>
        /// Get the paths of the directories in the directory with the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <returns>Child directory paths.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public PathBuilder[] GetDirectories(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!DirectoryExists(a_path))
                throw new DirectoryNotFoundException($"Directory at path \"{a_path}\" does not exist.");

            var paths = _directories.Keys.Where(i => IsChildDirectory(a_path, i))
                                         .Select(i => PathBuilder.Create(i)); ;

            return paths.ToArray();
        }


        /// <summary>
        /// Get the paths of the files in the directory with the given path (<paramref name="a_path"/>) that match the 
        ///     given search pattern (<paramref name="a_searchPattern"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <param name="a_searchPattern">Search pattern.</param>
        /// <returns>File paths.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_searchPattern"/> is null.</exception>
        public PathBuilder[] GetDirectories(string a_path, string a_searchPattern)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            if (a_searchPattern == null)
                throw new ArgumentNullException(nameof(a_searchPattern));

            #endregion

            var searchPattern = "^" + Regex.Escape(a_searchPattern).Replace("\\*", ".*") + "$";
            var rexSearch = new Regex(searchPattern, RegexOptions.IgnoreCase);

            var files = GetDirectories(a_path).Where(i => rexSearch.IsMatch(i.Name()));

            return files.ToArray();
        }

        /// <summary>
        /// Get an input stream for the file at the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">File path.</param>
        /// <returns>Input stream.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public Stream OpenRead(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            var instance = GetFileInstance(a_path);

            return new TestMemoryStream(instance);
        }

        /// <summary>
        /// get an output stream for the file at the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">File path.</param>
        /// <returns>Input stream.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public Stream OpenWrite(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            if (!FileExists(a_path))
                StageFile(a_path, new TestFileInstance());

            var instance = GetFileInstance(a_path);

            return new TestMemoryStream(instance);
        }

        /// <summary>
        /// Get the actual instance for the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Relative path.</param>
        /// <returns>File instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public TestFileInstance GetFileInstance(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            if (!FileExists(a_path))
                throw new FileNotFoundException();

            a_path = PreparePath(a_path);

            var pb = PathBuilder.Create(a_path).WithRoot(PathBuilder.WindowsDriveRoot);

            var directory = pb.Parent();

            if (directory == null)
                return null;

            var file = pb.Name();

            if (!DirectoryExists(directory))
                return null;

            var files = _directories[directory];

            if (files.ContainsKey(file))
                return files[file];

            return null;
        }

        /// <summary>
        /// Prepare the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">PathResult to prepare.</param>
        /// <returns>Prepared.</returns>
        protected virtual string PreparePath(string a_path)
        {
            if (!Regex.IsMatch(a_path, @"^(?:[a-zA-Z]\:\\|\\)"))
                throw new ArgumentException("PathResult is not rooted or invalid!", nameof(a_path));

            a_path = Path.GetFullPath(a_path);

            if (!Regex.IsMatch(a_path, @"^(?:[a-zA-Z]\:\\)$"))
                a_path = a_path.TrimEnd('\\');

            return a_path;
        }

        /// <summary>
        /// Determine if the other given directory (<paramref name="a_other"/>) is a direct child of the given parent directory (<paramref name="a_parent"/>).
        /// </summary>
        /// <param name="a_parent">Parent directory.</param>
        /// <param name="a_other">Other directory.</param>
        /// <returns>True is direct child.</returns>
        private bool IsChildDirectory(string a_parent, string a_other)
        {
            if (a_parent == null)
                return false;

            var othersParent = PathBuilder.Create(a_other).WithRoot(PathBuilder.WindowsDriveRoot).Parent()?.ToString();

            if (othersParent == null)
                return false;

            return othersParent.Equals(a_parent, StringComparison.OrdinalIgnoreCase);
        }

        class TestMemoryStream : MemoryStream
        {
            private readonly TestFileInstance _instance;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="a_instance">File instance.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_instance"/> is null.</exception>
            public TestMemoryStream(TestFileInstance a_instance)
            {
                #region Argument Validation

                if (a_instance == null)
                    throw new ArgumentNullException(nameof(a_instance));

                #endregion

                _instance = a_instance;

                if (a_instance.Data.Any())
                {
                    var writer = new BinaryWriter(this);
                    writer.Write(_instance.Data, 0, _instance.Data.Length);
                    writer.Flush();
                    Seek(0, SeekOrigin.Begin);
                }
            }

            /// <summary>
            /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream. Instead of calling this method, 
            /// ensure that the stream is properly disposed.
            /// </summary>
            public override void Close()
            {
                _instance.Size = Length;
                _instance.Data = new byte[Length];

                if (Length > 0)
                {
                    Seek(0, SeekOrigin.Begin);
                    var reader = new BinaryReader(this);
                    _instance.Data = reader.ReadBytes((int) Length);
                }

                base.Close();
            }
        }
    }
}