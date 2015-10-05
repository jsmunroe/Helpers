using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Helpers.Test
{
    public class TestFileSystem
    {
        private readonly Dictionary<string, Dictionary<string, TestFileStats>> _directories = new Dictionary<string, Dictionary<string, TestFileStats>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Create the directory with the given path (<paramref name="a_path"/>) within this file system.
        /// </summary>
        /// <param name="a_path">Directory to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <returns>Created test directory.</returns>
        public TestDirectory CreateDirectory(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!_directories.ContainsKey(a_path))
            {
                _directories.Add(a_path, new Dictionary<string, TestFileStats>(StringComparer.OrdinalIgnoreCase));

                var parent = Path.GetDirectoryName(a_path);
                if (parent != null)
                    CreateDirectory(parent);
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
        /// <param name="a_stats">File stats.</param>
        /// <returns>This file system used with fluent interface.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_stats"/> is null.</exception>
        public TestFile CreateFile(string a_path, TestFileStats a_stats)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            if (a_stats == null)
                throw new ArgumentNullException(nameof(a_stats));

            #endregion

            a_path = PreparePath(a_path);

            var directory = Path.GetDirectoryName(a_path);
            var file = Path.GetFileName(a_path);

            CreateDirectory(directory);

            var files = _directories[directory];
            files[file] = a_stats;

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

            var directory = Path.GetDirectoryName(a_path);
            var file = Path.GetFileName(a_path);

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

            var directory = Path.GetDirectoryName(a_path);
            var file = Path.GetFileName(a_path);

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
        public string[] GetFiles(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!DirectoryExists(a_path))
                throw new DirectoryNotFoundException($"Directory at path \"{a_path}\" does not exist.");

            var files = _directories[a_path].Keys.Select(i => Path.Combine(a_path, i));

            return files.ToArray();
        }

        /// <summary>
        /// Get the paths of the directories in the directory with the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <returns>Child directory paths.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public string[] GetDirectories(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!DirectoryExists(a_path))
                throw new DirectoryNotFoundException($"Directory at path \"{a_path}\" does not exist.");

            var paths = _directories.Keys.Where(i => IsChildDirectory(a_path, i));

            return paths.ToArray();
        }

        /// <summary>
        /// Get the stats for the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Relative path.</param>
        /// <returns>File stats.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public TestFileStats GetFileStats(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var directory = Path.GetDirectoryName(a_path);

            if (directory == null)
                return null;

            var file = Path.GetFileName(a_path);

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
        /// <param name="a_path">Path to prepare.</param>
        /// <returns>Prepared.</returns>
        protected virtual string PreparePath(string a_path)
        {
            if (!Regex.IsMatch(a_path, @"^(?:[a-zA-Z]\:\\|\\)"))
                throw new ArgumentException("Path is not rooted or invalid!", nameof(a_path));

            a_path = Path.GetFullPath(a_path);

            if (!Regex.IsMatch(a_path, @"^(?:[a-zA-Z]\:\\)"))
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

            var othersParent = Path.GetDirectoryName(a_other);

            if (othersParent == null)
                return false;

            return othersParent.Equals(a_parent, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class TestFileStats
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestFileStats()
        {
            CreatedTimeUtc = DateTime.UtcNow;
            LastModifiedTimeUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Size of the file.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}