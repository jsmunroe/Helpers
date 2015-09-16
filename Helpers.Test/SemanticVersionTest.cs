using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test
{
    [TestClass]
    public class SemanticVersionTest
    {
        [TestMethod]
        public void ConstructSemanticVersionWithParts()
        {
            // Execute
            var version = new SemanticVersion(1, 2, 3, "alpha");

            // Assert
            Assert.AreEqual(1, version.Major);
            Assert.AreEqual(2, version.Minor);
            Assert.AreEqual(3, version.Patch);
            Assert.AreEqual("alpha", version.Release);
            Assert.AreEqual("1.2.3-alpha", version.ToString());
        }


        [TestMethod]
        public void ConstructSemanticversionWithPartsAndNullRelease()
        {
            // Execute
            var version = new SemanticVersion(1, 2, 3, a_release: null);

            // Assert
            Assert.AreEqual(1, version.Major);
            Assert.AreEqual(2, version.Minor);
            Assert.AreEqual(3, version.Patch);
            Assert.AreEqual("", version.Release);
            Assert.AreEqual("1.2.3", version.ToString());
        }


        [TestMethod]
        public void ConstructSemanticVersionWithText_I()
        {
            // Execute
            var version = new SemanticVersion("2.1.0-beta3");

            // Assert
            Assert.AreEqual(2, version.Major);
            Assert.AreEqual(1, version.Minor);
            Assert.AreEqual(0, version.Patch);
            Assert.AreEqual("beta3", version.Release);
            Assert.AreEqual("2.1.0-beta3", version.ToString());
        }

        [TestMethod]
        public void ConstructSemanticVersionWithNoRelease()
        {
            // Execute
            var version = new SemanticVersion("2.1.0");

            // Assert
            Assert.AreEqual(2, version.Major);
            Assert.AreEqual(1, version.Minor);
            Assert.AreEqual(0, version.Patch);
            Assert.AreEqual("", version.Release);
            Assert.AreEqual("2.1.0", version.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConstructSemanticversionWithBadText_I()
        {
            // Execute
            var version = new SemanticVersion("3.6-beta3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConstructSemanticversionWithBadText_II()
        {
            // Execute
            var version = new SemanticVersion("3.6.9rc1");
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConstructSemanticversionWithBadText_III()
        {
            // Execute
            var version = new SemanticVersion("3.6.9-1");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructSemanticVersionWithNullText()
        {
            // Execute
            var version = new SemanticVersion(a_version: null);
        }

        [TestMethod]
        public void Parse()
        {
            // Execute
            var version = SemanticVersion.Parse("2.1.0-beta-3");

            // Assert
            Assert.AreEqual(2, version.Major);
            Assert.AreEqual(1, version.Minor);
            Assert.AreEqual(0, version.Patch);
            Assert.AreEqual("beta-3", version.Release);
            Assert.AreEqual("2.1.0-beta-3", version.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseWithBadText_I()
        {
            // Execute
            var version = SemanticVersion.Parse("3.6.9rc1");
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseithBadText_II()
        {
            // Execute
            var version = SemanticVersion.Parse("3.6.9-rc.1");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseWithNull()
        {
            // Execute
            var version = SemanticVersion.Parse(a_version: null);
        }

        [TestMethod]
        public void TryParse()
        {
            // Execute
            SemanticVersion version;
            var result = SemanticVersion.TryParse("2.1.0-beta-3", out version);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryParseWithBadText_I()
        {
            // Execute
            SemanticVersion version;
            var result = SemanticVersion.TryParse("3.6.9rc1", out version);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryParseithBadText_II()
        {
            // Execute
            SemanticVersion version;
            var result = SemanticVersion.TryParse("3.6.9-rc.1", out version);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryParseWithNull()
        {
            // Execute
            SemanticVersion version;
            var result = SemanticVersion.TryParse(a_version: null, a_value: out version);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConvertToClassicalVersion()
        {
            // Setup
            var version = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version.ToVersion();

            // Assert
            Assert.AreEqual(4, result.Major);
            Assert.AreEqual(15, result.Minor);
            Assert.AreEqual(26, result.Build);
            Assert.AreEqual(-1, result.Revision);
        }

        [TestMethod]
        public void CompareTo_I()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1.CompareTo(a_obj: version2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareTo_II()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1.CompareTo(version2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_III()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1.CompareTo(version2);

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_IV()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta7");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1.CompareTo(version2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareTo_V()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha9");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1.CompareTo(version2);

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_VI()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1.CompareTo(version2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareToWithNull()
        {
            // Setup
            var version = new SemanticVersion("1.0.0");

            // Execute
            var result = version.CompareTo(null);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareToWithWrongType()
        {
            // Setup
            var version = new SemanticVersion("1.0.0");

            // Execute
            var result = version.CompareTo("This is not a SemanticVersion!");
        }

        [TestMethod]
        public void OporatorEqualTo()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1 == version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OporatorEqualToWhereNotEqual()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 == version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OporatorEqualToNull()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1 == null;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OporatorEqualToNullReversed()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");

            // Execute
            var result = null == version1;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OporatorNotEqualTo()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 != version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OporatorNotEqualToWhereEqual()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1 != version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OporatorNotEqualToNull()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1 != null;

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void OporatorNotEqualToNullReversed()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");

            // Execute
            var result = null != version1;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorGreaterThan()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1 > version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorGreaterThanWhereEqual()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 > version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OperatorGreaterThanWhereLessThan()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");
            var version2 = new SemanticVersion("4.15.26-alpha1");

            // Execute
            var result = version1 > version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorGreaterThanNull()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = version1 > null;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorGreaterThanNullReversed()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = null > version1;
        }

        [TestMethod]
        public void OperatorGreaterThanOrEqualTo()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("1.0.0");

            // Execute
            var result = version1 >= version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorGreaterThanOrEqualToWhereEqual()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 >= version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorGreaterThanOrEqualToWhereLessThan()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");
            var version2 = new SemanticVersion("4.15.26-alpha1");

            // Execute
            var result = version1 >= version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorGreaterThanOrEqualToNull()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = version1 >= null;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorGreaterThanOrEqualToNullReversed()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = null >= version1;
        }

        [TestMethod]
        public void OperatorLessThan()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 < version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorLessThanWhereEqual()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 < version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OperatorLessThanWhereGreaterThan()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha1");
            var version2 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = version1 < version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorLessThanNull()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = version1 < null;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorLessThanNullReversed()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = null < version1;
        }

        [TestMethod]
        public void OperatorLessThanOrEqualTo()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 <= version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorLessThanOrEqualToWhereEqual()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion("4.15.26-beta6");

            // Execute
            var result = version1 <= version2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OperatorLessThanOrEqualToWhereGreaterThan()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha1");
            var version2 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = version1 <= version2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorLessThanOrEqualToNull()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = version1 <= null;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorLessThanOrEqualToNullReversed()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-alpha");

            // Execute
            var result = null <= version1;
        }


        [TestMethod]
        public void CallToString()
        {
            // Setup
            var version1 = new SemanticVersion(1, 2, 3, "rc3");

            // Execute
            var result = version1.ToString();

            // Assert
            Assert.AreEqual("1.2.3-rc3", result);
        }

        [TestMethod]
        public void Equals()
        {
            // Setup
            var version1 = new SemanticVersion("4.15.26-beta6");
            var version2 = new SemanticVersion(4, 15, 26, "beta6");

            // Execute
            var result = version1.Equals(version2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsWhereNot()
        {
            // Setup
            var version1 = new SemanticVersion("1.0.0-alpha");
            var version2 = new SemanticVersion(4, 15, 26, "beta6");

            // Execute
            var result = version1.Equals(version2);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
