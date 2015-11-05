using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Helpers.IO;

namespace Helpers.Collections
{
    public class PathTree<TLeaf>
    {
        private readonly Dictionary<string, Dictionary<string, TLeaf>> _directories = new Dictionary<string, Dictionary<string, TLeaf>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Create the directory with the given path (<paramref name="a_path"/>) within this file system.
        /// </summary>
        /// <param name="a_path">Directory to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <returns>Created path directory.</returns>
        public PathDirectory<TLeaf> CreateDirectory(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            if (!_directories.ContainsKey(a_path))
            {
                _directories.Add(a_path, new Dictionary<string, TLeaf>(StringComparer.OrdinalIgnoreCase));

                var path = PathBuilder.Create(a_path);
                if (Regex.IsMatch(a_path, @"^[A-Za-z]\:\\"))
                    path = path.WithRoot(PathBuilder.WindowsDriveRoot);

                var parent = path.Parent();
                if (parent != null)
                    CreateDirectory(parent);
            }

            return new PathDirectory<TLeaf>(this, a_path);
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

            if (a_path == "")
                return _directories.Any(i => !Regex.IsMatch(a_path, @"^([A-Za-z]\:\\|\\)"));

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
        /// <param name="a_value">File value.</param>
        /// <returns>This file system used with fluent interface.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_value"/> is null.</exception>
        public PathFile<TLeaf> CreateFile(string a_path, TLeaf a_value)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var path = PathBuilder.Create(a_path);
            if (Regex.IsMatch(a_path, @"^[A-Za-z]\:\\"))
                path = path.WithRoot(PathBuilder.WindowsDriveRoot);

            var directory = path.Parent() ?? "";
            var file = path.Name();

            CreateDirectory(directory);

            var files = _directories[directory];
            files[file] = a_value;

            return new PathFile<TLeaf>(this, a_path);
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

            var path = PathBuilder.Create(a_path);
            if (Regex.IsMatch(a_path, @"^[A-Za-z]\:\\"))
                path = path.WithRoot(PathBuilder.WindowsDriveRoot);

            var directory = path.Parent() ?? "";
            var file = path.Name();

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

            var path = PathBuilder.Create(a_path);
            if (Regex.IsMatch(a_path, @"^[A-Za-z]\:\\"))
                path = path.WithRoot(PathBuilder.WindowsDriveRoot);

            var directory = path.Parent();
            var file = path.Name();

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
        /// Get the paths of the files in the directory with the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <param name="a_searchPattern">Search pattern.</param>
        /// <returns>File paths.</returns>
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
                                         .Select(i => PathBuilder.Create(i));

            return paths.ToArray();
        }

        /// <summary>
        /// Get the paths of the directories in the directory with the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Directory path.</param>
        /// <returns>Child directory paths.</returns>
        /// <param name="a_searchPattern">Search pattern.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
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
        /// Get the stats for the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Relative path.</param>
        /// <returns>File stats.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        public TLeaf GetLeafValue(string a_path)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            #endregion

            a_path = PreparePath(a_path);

            var path = PathBuilder.Create(a_path);
            if (Regex.IsMatch(a_path, @"^[A-Za-z]\:\\"))
                path = path.WithRoot(PathBuilder.WindowsDriveRoot);

            var directory = path.Parent();

            if (directory == null)
                return default(TLeaf);

            var file = path.Name();

            if (!DirectoryExists(directory))
                return default(TLeaf);

            var files = _directories[directory];

            if (files.ContainsKey(file))
                return files[file];

            return default(TLeaf);
        }

        /// <summary>
        /// Prepare the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">PathResult to prepare.</param>
        /// <returns>Prepared.</returns>
        protected virtual string PreparePath(string a_path)
        {
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

            var path = PathBuilder.Create(a_other);
            if (Regex.IsMatch(a_other, @"^[A-Za-z]\:\\"))
                path = path.WithRoot(PathBuilder.WindowsDriveRoot);

            var othersParent = path.Parent()?.ToString();

            if (othersParent == null && a_parent == "" && a_other != "" && !a_other.Contains('\\')) // "" is a parent of "Sub"
                return true;
            else if (othersParent == null)
                return false;

            return othersParent.Equals(a_parent, StringComparison.OrdinalIgnoreCase);
        }

    }
}