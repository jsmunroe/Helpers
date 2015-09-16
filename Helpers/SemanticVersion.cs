using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers
{
    // Borrowed from "https://nuget.codeplex.com/SourceControl/latest#src/Core/SemanticVersionTypeConverter.cs"
    //  with extensive refactoring.

    [Serializable]
    [TypeConverter(typeof(SemanticVersionTypeConverter))]
    public sealed class SemanticVersion : IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        private static readonly Regex s_rexSemanticVersion = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<release>[a-z][0-9a-z-]*))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        private readonly string _originalString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_version">String samantic version.</param>
        public SemanticVersion(string a_version)
            : this(Parse(a_version))
        {
            _originalString = a_version;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_major">Major version.</param>
        /// <param name="a_minor">Minor version.</param>
        /// <param name="a_patch">Path version.</param>
        /// <param name="a_release">Special pre-release and build metadata.</param>
        public SemanticVersion(int a_major, int a_minor, int a_patch, string a_release = null)
        {
            Version = new Version(a_major, a_minor, a_patch);
            Release = a_release ?? "";

            _originalString = Version + (!string.IsNullOrEmpty(a_release) ? '-' + a_release : "");

        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="a_other">Other semantic version.</param>
        public SemanticVersion(SemanticVersion a_other)
        {
            _originalString = a_other._originalString;
            Version = a_other.Version;
            Release = a_other.Release;
        }

        /// <summary>
        /// Normalized version portion containing major, minor, and patch/build.
        /// </summary>
        private Version Version { get; }

        /// <summary>
        /// Major version.
        /// </summary>
        public int Major => Version.Major;

        /// <summary>
        /// Minor version.
        /// </summary>
        public int Minor => Version.Minor;

        /// <summary>
        /// Patch version.
        /// </summary>
        public int Patch => Version.Build;

        /// <summary>
        /// Special pre-release and build metadata.
        /// </summary>
        public string Release { get; }


        /// <summary>
        /// Parse the given version text (<paramref name="a_version"/>) into a semantic version.
        /// </summary>
        /// <param name="a_version">Version text.</param>
        /// <returns>Semantic version.</returns>
        /// <exception cref="FormatException">Thrown if <paramref name="a_version"/> is badly formatted..</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_version"/> is null.</exception>
        public static SemanticVersion Parse(string a_version)
        {
            #region Argument Validation

            if (a_version == null)
                throw new ArgumentNullException(nameof(a_version));

            #endregion

            SemanticVersion semanticVersion;
            if (!TryParse(a_version, out semanticVersion))
                throw new FormatException($"Given version \"{a_version}\" is badly formatted.");

            return semanticVersion;
        }

        /// <summary>
        /// Try to parse the given version text (<paramref name="a_version"/>) into a semantic version.
        /// </summary>
        /// <param name="a_version">Version text.</param>
        /// <param name="a_value">(output) Parsed semantic version.</param>
        /// <returns>True if successfully parsed.</returns>
        public static bool TryParse(string a_version, out SemanticVersion a_value)
        {
            a_value = null;
            if (string.IsNullOrEmpty(a_version))
                return false;

            var match = s_rexSemanticVersion.Match(a_version.Trim());
            if (!match.Success)
                return false;

            var major = int.Parse(match.Groups["major"].Value);
            var minor = int.Parse(match.Groups["minor"].Value);
            var patch = int.Parse(match.Groups["patch"].Value);
            var release = match.Groups["release"].Value;

            a_value = new SemanticVersion(major, minor, patch, release);

            return true;
        }

        /// <summary>
        /// Convert this semantic version to a classical version containing major, minor, patch as the build, and release iff release is a number as the revision.
        /// </summary>
        /// <returns>Classical version.</returns>
        public Version ToVersion()
        {
            int revision;
            if (int.TryParse(Release, out revision))
                return new Version(Major, Minor, Patch, revision);
            else
                return new Version(Major, Minor, Patch);
        }

        /// <summary>
        /// Compare this semantic version to the given object (<paramref name="a_obj"/>).
        /// </summary>
        /// <param name="a_obj">Object.</param>
        /// <returns>Precedence between this semantic version and the other given object.</returns>
        public int CompareTo(object a_obj)
        {
            if (ReferenceEquals(a_obj, null))
                return 1;

            var other = a_obj as SemanticVersion;
            if (other == null)
                throw new ArgumentException("Object is null or not a SemanticVersion.", nameof(a_obj));

            return CompareTo(other);
        }

        /// <summary>
        /// Compare this semantic version to the given semantic version. (<paramref name="a_other"/>).
        /// </summary>
        /// <param name="a_other">Other semantic version.</param>
        /// <returns>Precedence between this and the other given semantic version.</returns>
        public int CompareTo(SemanticVersion a_other)
        {
            if (ReferenceEquals(a_other, null))
                return 1;

            int result = Version.CompareTo(a_other.Version);

            if (result != 0)
                return result;

            var empty = string.IsNullOrEmpty(Release);
            var otherEmpty = string.IsNullOrEmpty(a_other.Release);
            if (empty && otherEmpty)
                return 0;
            else if (empty)
                return 1;
            else if (otherEmpty)
                return -1;

            return StringComparer.OrdinalIgnoreCase.Compare(Release, a_other.Release);
        }

        /// <summary>
        /// Operator overload '=='.
        /// </summary>
        /// <param name="a_first">First version.</param>
        /// <param name="a_second">Second version.</param>
        /// <returns>True iff the versions are value-equivelent.</returns>
        public static bool operator ==(SemanticVersion a_first, SemanticVersion a_second)
        {
            if (ReferenceEquals(a_first, null))
                return ReferenceEquals(a_second, null);

            return a_first.Equals(a_second);
        }

        /// <summary>
        /// Operator overload '!='.
        /// </summary>
        /// <param name="a_first">First version.</param>
        /// <param name="a_second">Second version.</param>
        /// <returns>True iff the versions are not value-equivelent.</returns>
        public static bool operator !=(SemanticVersion a_first, SemanticVersion a_second)
        {
            return !(a_first == a_second);
        }

        /// <summary>
        /// Operator overload '&lt;'.
        /// </summary>
        /// <param name="a_first">First version.</param>
        /// <param name="a_second">Second version.</param>
        /// <returns>True iff the first version is less than the second.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_first"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_second"/> is null.</exception>
        public static bool operator <(SemanticVersion a_first, SemanticVersion a_second)
        {
            #region Argument Validation

            if (a_first == null)
                throw new ArgumentNullException(nameof(a_first));

            if (a_second == null)
                throw new ArgumentNullException(nameof(a_second));

            #endregion

            return a_first.CompareTo(a_second) < 0;
        }

        /// <summary>
        /// Operator overload '&lt;='.
        /// </summary>
        /// <param name="a_first">First version.</param>
        /// <param name="a_second">Second version.</param>
        /// <returns>True iff the first version is less than or equal to the second.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_first"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_second"/> is null.</exception>
        public static bool operator <=(SemanticVersion a_first, SemanticVersion a_second)
        {
            return (a_first == a_second) || (a_first < a_second);
        }

        /// <summary>
        /// Operator overload '&gt;'.
        /// </summary>
        /// <param name="a_first">First version.</param>
        /// <param name="a_second">Second version.</param>
        /// <returns>True iff the first version is greater than the second.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_first"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_second"/> is null.</exception>
        public static bool operator >(SemanticVersion a_first, SemanticVersion a_second)
        {
            #region Argument Validation

            if (a_first == null)
                throw new ArgumentNullException(nameof(a_first));

            if (a_second == null)
                throw new ArgumentNullException(nameof(a_second));

            #endregion

            return a_second < a_first;
        }

        /// <summary>
        /// Operator overload '&gt;='.
        /// </summary>
        /// <param name="a_first">First version.</param>
        /// <param name="a_second">Second version.</param>
        /// <returns>True iff the first version is greater than or equal to the second.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_first"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_second"/> is null.</exception>
        public static bool operator >=(SemanticVersion a_first, SemanticVersion a_second)
        {
            return (a_first == a_second) || (a_first > a_second);
        }

        /// <summary>
        /// Convert this semantic version to a string representation.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return _originalString;
        }

        /// <summary>
        /// Determine if the other given semantic version (<paramref name="a_other"/>) is value-equal to this one.
        /// </summary>
        /// <param name="a_other">Other semantic version.</param>
        /// <returns>True iff both semantic versions are value-equal.</returns>
        public bool Equals(SemanticVersion a_other)
        {
            return !ReferenceEquals(null, a_other) &&
                   Version.Equals(a_other.Version) &&
                   Release.Equals(a_other.Release, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determine if the given object (<paramref name="a_obj"/>) is value-equal to this semantic version.
        /// </summary>
        /// <param name="a_obj">Object.</param>
        /// <returns>True iff object is value-equal to this semantic version.</returns>
        public override bool Equals(object a_obj)
        {
            var other = a_obj as SemanticVersion;

            return !ReferenceEquals(null, other) && Equals(other);
        }

        /// <summary>
        /// Get a hash code appropriate to this instance.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            int hashCode = Version.GetHashCode();
            if (Release != null)
                hashCode = hashCode * 4567 + Release.GetHashCode();

            return hashCode;
        }
    }

    public class SemanticVersionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            SemanticVersion semanticVersion;
            if (SemanticVersion.TryParse(value as string, out semanticVersion))
                return semanticVersion;

            return null;
        }
    }
}
