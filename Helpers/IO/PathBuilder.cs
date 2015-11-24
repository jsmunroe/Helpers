using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers.IO
{
    public class PathBuilder
    {
        private readonly string _path;
        private readonly string _delimiter;

        // Options
        private string _rootPattern;
        private bool _padRoot = true;

        public static readonly string WindowsDriveRoot = @"^[A-Za-z]\:\\?$";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_path">Path value.</param>
        /// <param name="a_delimiter">Path delimiter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_delimiter"/> is null.</exception>
        public PathBuilder(string a_path, string a_delimiter = "\\")
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            if (a_delimiter == null)
                throw new ArgumentNullException(nameof(a_delimiter));

            #endregion

            _path = a_path;
            _delimiter = a_delimiter;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_other">Other path builder.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_other"/> is null.</exception>
        protected PathBuilder(PathBuilder a_other)
        {
            #region Argument Validation

            if (a_other == null)
                throw new ArgumentNullException(nameof(a_other));

            #endregion

            _path = a_other._path;
            _delimiter = a_other._delimiter;
            _rootPattern = a_other._rootPattern;
            _padRoot = a_other._padRoot;
        }

        /// <summary>
        /// Get all ancestor paths to this path from parent to child.
        /// </summary>
        public PathBuilder[] Ancestors
        {
            get
            {
                var ancestors = new List<PathBuilder>();
                var path = Parent();
                while (path != null)
                {
                    ancestors.Add(path);
                    path = path.Parent();
                }

                ancestors.Reverse();
                return ancestors.ToArray();
            }
        }

        /// <summary>
        /// Get all ancestor paths and this path to this path from parent to child.
        /// </summary>
        public PathBuilder[] AncestorsAndSelf
        {
            get
            {
                var ancestors = new List<PathBuilder>();
                var path = this;
                while (path != null)
                {
                    ancestors.Add(path);
                    path = path.Parent();
                }

                ancestors.Reverse();
                return ancestors.ToArray();
            }
        }

        /// <summary>
        /// Create a new path builder with the given path value (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">PathResult value.</param>
        /// <param name="a_delimiter">PathResult delimiter.</param>
        /// <returns>Created path.</returns>
        public static PathBuilder Create(string a_path, string a_delimiter = "\\")
        {
            return new PathBuilder(a_path, a_delimiter);
        }

        /// <summary>
        /// Created a new path builder with a root identity pattern (<paramref name="a_rootPattern"/>).
        /// </summary>
        /// <param name="a_rootPattern">Root identity pattern.</param>
        /// <param name="a_padRoot">Whether or not to pad the root with a delimiter.</param>
        /// <returns>Created path.</returns>
        public PathBuilder WithRoot(string a_rootPattern, bool a_padRoot = true)
        {
            var path = new PathBuilder(this);
            path._rootPattern = a_rootPattern;
            path._padRoot = a_padRoot;

            return path;
        }

        /// <summary>
        /// Add a relative path to this path to create a child / descendent path.
        /// </summary>
        /// <param name="a_relativePath">Relative path.</param>
        /// <returns>Created child path.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_relativePath"/> is null.</exception>
        public PathBuilder Child(string a_relativePath)
        {
            #region Argument Validation

            if (a_relativePath == null)
                throw new ArgumentNullException(nameof(a_relativePath));

            #endregion

            if (a_relativePath == "")
                return this;

            if (a_relativePath == _delimiter)
                return this;

            var pathValue = "";

            if (_path == "")
                pathValue = a_relativePath;
            else if (_path.EndsWith(_delimiter))
                pathValue = _path + a_relativePath;
            else
                pathValue = _path + _delimiter + a_relativePath;

            pathValue = pathValue.Replace(_delimiter + _delimiter, _delimiter);

            var path = new PathBuilder(pathValue, _delimiter);

            return ApplyOptions(path);
        }

        /// <summary>
        /// Get a sibling of this path's element with the given name (<paramref name="a_siblingName"/>).
        /// </summary>
        /// <param name="a_siblingName">Sibling name.</param>
        /// <returns>Created sibling path.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_siblingName"/> is null.</exception>
        public PathBuilder Sibling(string a_siblingName)
        {
            #region Argument Validation

            if (a_siblingName == null)
                throw new ArgumentNullException(nameof(a_siblingName));

            #endregion

            if (IsRoot(_path))
                return null;

            var parent = Parent();

            if (parent == null)
                parent = ApplyOptions(new PathBuilder(""));

            return parent.Child(a_siblingName);
        }

        /// <summary>
        /// Get the parent element's path.
        /// </summary>
        /// <returns>Parent path.</returns>
        public PathBuilder Parent()
        {
            if (IsRoot(_path))
                return null;

            var lastIndex = _path.LastIndexOf(_delimiter, StringComparison.Ordinal);

            if (lastIndex >= _path.Length - 1)
                lastIndex = _path.LastIndexOf(_delimiter, lastIndex - 1, StringComparison.Ordinal);

            if (lastIndex < 0)
                return ApplyOptions(Create(""));

            var value = _path.Substring(0, lastIndex);

            if (IsRoot(value) && _padRoot)
                value += _delimiter;

            var path =  new PathBuilder(value, _delimiter);

            return ApplyOptions(path);
        }

        /// <summary>
        /// Get the name of the last segment in this path.
        /// </summary>
        /// <returns>Name of the last segment.</returns>
        public string Name()
        {
            if (_path == "")
                return null;

            if (IsRoot(_path))
                return "";

            var lastIndex = _path.LastIndexOf(_delimiter, StringComparison.Ordinal);

            if (lastIndex >= _path.Length - 1)
                lastIndex = _path.LastIndexOf(_delimiter, lastIndex - 1, StringComparison.Ordinal);

            if (lastIndex < 0)
                return _path;

            var value = _path.Substring(lastIndex + 1);

            if (value.EndsWith(_delimiter))
                value = value.Substring(0, value.Length - _delimiter.Length);

            return value;
        }

        /// <summary>
        /// Get the name of the last segment in this path without its extension.
        /// </summary>
        /// <param name="a_extensionMarker">String that indicates the start of an extension.</param>
        /// <returns>Name of the last segment without its extension.</returns>
        public string NameWithoutExtension(string a_extensionMarker = ".")
        {
            var name = Name();

            if (name == null)
                return null;

            var lastIndex = name.LastIndexOf(a_extensionMarker, StringComparison.Ordinal);
            if (lastIndex < 0)
                return name;

            var value = name.Substring(0, lastIndex);

            return value;
        }

        /// <summary>
        /// Get the extension of the last segment with its marker (".").
        /// </summary>
        /// <param name="a_extensionMarker">String that indicates the start of an extension.</param>
        /// <returns>Extension of the last segment.</returns>
        public string Extension(string a_extensionMarker = ".")
        {
            var name = Name();

            if (name == null)
                return null;

            var lastIndex = name.LastIndexOf(a_extensionMarker, StringComparison.Ordinal);
            if (lastIndex < 0)
                return name;

            var value = name.Substring(lastIndex);

            return value;
        }

        /// <summary>
        /// Change the extension of the last segment to the given extension (<paramref name="a_extension"/>).
        /// </summary>
        /// <param name="a_extension">New extension.</param>
        /// <param name="a_extensionMarker">String that indicates the start of an extension.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_extension"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_extensionMarker"/> is null.</exception>
        public PathBuilder ChangeExtension(string a_extension, string a_extensionMarker = ".")
        {
            #region Argument Validation

            if (a_extension == null)
                throw new ArgumentNullException(nameof(a_extension));

            if (a_extensionMarker == null)
                throw new ArgumentNullException(nameof(a_extensionMarker));

            #endregion

            var parent = Parent();
            var name = NameWithoutExtension(a_extensionMarker) + a_extensionMarker + a_extension;

            var path = parent + name;

            return path;
        }

        /// <summary>
        /// Whether this path is at root.
        /// </summary>
        /// <returns>True iff this path is at root.</returns>
        private bool IsRoot(string a_path)
        {
            if (_rootPattern == null)
                return (a_path == "" || a_path == _delimiter);
            else
                return Regex.IsMatch(a_path, _rootPattern);
        }

        /// <summary>
        /// Apply options in this path builder to the given one (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Path builder to which to apply options.</param>
        /// <returns>Given path builder.</returns>
        private PathBuilder ApplyOptions(PathBuilder a_path)
        {
            a_path._rootPattern = _rootPattern;
            a_path._padRoot = _padRoot;

            return a_path;
        }

        /// <summary>
        /// Returns current path value.
        /// </summary>
        /// <returns>A string containing current path value.</returns>
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Get the relative path between given paths (<paramref name="a_first"/>, <paramref name="a_second"/>).
        /// </summary>
        /// <param name="a_first">First path.</param>
        /// <param name="a_second">Second path.</param>
        /// <returns>
        /// Relative path.
        /// Empty string if the paths are the same.
        /// Null if there is no relationship.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_first"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_second"/> is null.</exception>
        public static string Relative(PathBuilder a_first, PathBuilder a_second)
        {
            #region Argument Validation

            if (a_first == null)
                throw new ArgumentNullException(nameof(a_first));

            if (a_second == null)
                throw new ArgumentNullException(nameof(a_second));

            #endregion

            var first = a_first.ToString().ToLower();
            var second = a_second.ToString().ToLower();

            if (a_first._delimiter != a_second._delimiter)
                throw new InvalidOperationException("Delimiter mismatch.");

            if (first == second)
                return "";

            if (first.StartsWith(second))
                return Relative(a_second, a_first);

            if (!second.StartsWith(first))
                return null;

            var baseLength = first.Length;
            var relative = second.Substring(baseLength);

            while (relative.StartsWith(a_first._delimiter))
                relative = relative.Substring(a_second._delimiter.Length);

            return relative;
        }

        /// <summary>
        /// Determine whether the other given object (<paramref name="obj"/>) is equal to this one.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True iff objects are equal.</returns>
        public override bool Equals(object obj)
        {
            // See the full list of guidelines at http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at http://go.microsoft.com/fwlink/?LinkId=85238

            if (obj == null || GetType() != obj.GetType())
                return false;

            if (Equals(obj as PathBuilder))
                return true;

            if (Equals(obj as string))
                return true;

            return base.Equals(obj);
        }

        /// <summary>
        /// Determine whether the other given path (<paramref name="a_other"/>) is equal to this one.
        /// </summary>
        /// <param name="a_other">Other path.</param>
        /// <returns>True iff paths are equal.</returns>
        public bool Equals(PathBuilder a_other)
        {
            if (a_other == null)
                return false;

            return _path.Equals(a_other._path, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determine whether the other given path (<paramref name="a_other"/>) is equal to this one.
        /// </summary>
        /// <param name="a_other">Other path.</param>
        /// <returns>True iff paths are equal.</returns>
        public bool Equals(string a_other)
        {
            if (a_other == null)
                return false;

            return _path.Equals(a_other, StringComparison.OrdinalIgnoreCase);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return _path.ToLower().GetHashCode();
        }

        /// <summary>
        /// Implicit cast operator to string.
        /// </summary>
        /// <param name="a_pathBuilder">Full path instance.</param>
        public static implicit operator string (PathBuilder a_pathBuilder)
        {
            return a_pathBuilder?.ToString();
        }

        /// <summary>
        /// Explicit cast operator from string.
        /// </summary>
        /// <param name="a_path">Full path value.</param>
        public static explicit operator PathBuilder(string a_path)
        {
            return Create(a_path);
        }

        /// <summary>
        /// Add the given segment (<paramref name="a_segment"/>) to the end of the given path (<paramref name="a_path"/>).
        /// </summary>
        /// <param name="a_path">Path.</param>
        /// <param name="a_segment">Segment.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_path"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_segment"/> is null.</exception>
        public static PathBuilder operator +(PathBuilder a_path, string a_segment)
        {
            #region Argument Validation

            if (a_path == null)
                throw new ArgumentNullException(nameof(a_path));

            if (a_segment == null)
                throw new ArgumentNullException(nameof(a_segment));

            #endregion

            var delimiter = a_path._delimiter;
            var path = a_path.ToString();

            while (path.EndsWith(delimiter))
                path = path.Substring(0, path.Length - delimiter.Length);

            while (a_segment.StartsWith(delimiter))
                a_segment = a_segment.Substring(delimiter.Length);

            path = path + delimiter + a_segment;

            return new PathBuilder(path);
        }

    }
}
