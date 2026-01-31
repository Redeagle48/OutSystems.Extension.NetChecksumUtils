using NUnit.Framework;
using OutSystems.NetChecksumUtils;
using System;
using System.Text;
using System.Security.Cryptography;

namespace OutSystems.NetChecksumUtils.Tests
{
    /// <summary>
    /// Comprehensive tests for <see cref="NetChecksumUtils"/>.
    /// Validates hashing accuracy, performance tracking (ticks), and input validation.
    /// </summary>
    [TestFixture]
    public class NetChecksumUtilsTests
    {
        private readonly NetChecksumUtils _sut = new();

        #region Helper Methods

        /// <summary>
        /// Reference implementation to verify the SUT output against standard .NET providers.
        /// Matches the internal logic of the SUT for cross-validation.
        /// </summary>
        private static string GetExpectedHash(string algorithm, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            return algorithm.Trim().ToUpperInvariant() switch
            {
                "SHA256" or "SHA-256" => Convert.ToHexString(SHA256.HashData(data)),
                "SHA512" or "SHA-512" => Convert.ToHexString(SHA512.HashData(data)),
                "SHA3_256" or "SHA3-256" => Convert.ToHexString(SHA3_256.HashData(data)),
                "MD5" => Convert.ToHexString(MD5.HashData(data)),
                _ => throw new ArgumentException("Unsupported algorithm in test helper", nameof(algorithm))
            };
        }

        private static string GetExpectedBase64(string algorithm, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            return algorithm.Trim().ToUpperInvariant() switch
            {
                "SHA256" or "SHA-256" => Convert.ToBase64String(SHA256.HashData(data)),
                "SHA512" or "SHA-512" => Convert.ToBase64String(SHA512.HashData(data)),
                "SHA3_256" or "SHA3-256" => Convert.ToBase64String(SHA3_256.HashData(data)),
                "MD5" => Convert.ToBase64String(MD5.HashData(data)),
                _ => throw new ArgumentException("Unsupported algorithm in test helper", nameof(algorithm))
            };
        }

        #endregion

        #region ComputeChecksum Tests

        /// <summary>
        /// Validates that all supported algorithms produce the correct hash and record a valid duration.
        /// </summary>
        [TestCase("SHA256", "OutSystems")]
        [TestCase("SHA-256", "OutSystems")]
        [TestCase("SHA512", "High performance low-code")]
        [TestCase("SHA3_256", "Modern security")]
        [TestCase("MD5", "Legacy support")]
        [TestCase("SHA256", "")] // Edge case: empty string
        public void ComputeChecksum_ValidInput_ReturnsCorrectHash(string algorithm, string text)
        {
            // Arrange
            string expected = GetExpectedHash(algorithm, text);

            // Act
            _sut.ComputeChecksum(algorithm, text, out string actualHex, out string actualBase64, out long ticks);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualHex, Is.EqualTo(expected), $"Hex hash mismatch for {algorithm}");
                Assert.That(actualBase64, Is.EqualTo(GetExpectedBase64(algorithm, text)), $"Base64 hash mismatch for {algorithm}");
                Assert.That(ticks, Is.GreaterThanOrEqualTo(0), "Duration should be non-negative.");
            });
        }

        #endregion

        #region VerifyChecksum Tests

        /// <summary>
        /// Validates that VerifyChecksum correctly identifies matching hashes (case-insensitive) 
        /// and detects mismatched content.
        /// </summary>
        [Test]
        public void VerifyChecksum_CorrectHash_ReturnsTrue()
        {
            const string algo = "SHA256";
            const string text = "Check me";
            string validHash = GetExpectedHash(algo, text);

            _sut.VerifyChecksum(algo, text, validHash, out bool isValid, out _);

            Assert.That(isValid, Is.True, "Verification should pass for matching hash.");
        }

        [Test]
        public void VerifyChecksum_MismatchedHash_ReturnsFalse()
        {
            const string algo = "SHA256";
            const string text = "Check me";
            const string wrongHash = "A1B2C3D4E5F6"; // Clearly wrong

            _sut.VerifyChecksum(algo, text, wrongHash, out bool isValid, out _);

            Assert.That(isValid, Is.False, "Verification should fail for incorrect hash.");
        }

        [Test]
        public void VerifyChecksum_CaseInsensitiveHash_ReturnsTrue()
        {
            const string algo = "SHA256";
            const string text = "Check me";
            string lowerHash = GetExpectedHash(algo, text).ToLowerInvariant();

            _sut.VerifyChecksum(algo, text, lowerHash, out bool isValid, out _);

            Assert.That(isValid, Is.True, "Verification should be case-insensitive.");
        }

        #endregion

        #region Exception & Error Handling

        /// <summary>
        /// Ensures ArgumentNullException is thrown when any required input is null.
        /// </summary>
        [Test]
        public void Methods_NullInputs_ThrowArgumentNullException()
        {
            Assert.Multiple(() =>
            {
                // ComputeChecksum null checks
                Assert.Throws<ArgumentNullException>(() => _sut.ComputeChecksum(null!, "text", out _, out _, out _));
                Assert.Throws<ArgumentNullException>(() => _sut.ComputeChecksum("SHA256", null!, out _, out _, out _));

                // VerifyChecksum null checks
                Assert.Throws<ArgumentNullException>(() => _sut.VerifyChecksum(null!, "text", "hash", out _, out _));
                Assert.Throws<ArgumentNullException>(() => _sut.VerifyChecksum("SHA256", null!, "hash", out _, out _));
                Assert.Throws<ArgumentNullException>(() => _sut.VerifyChecksum("SHA256", "text", null!, out _, out _));
            });
        }

        /// <summary>
        /// Ensures ArgumentException is thrown for unsupported algorithm strings.
        /// </summary>
        [TestCase("SHA1")] // Specifically not implemented in your GetHashFunction
        [TestCase("ROT13")]
        [TestCase("")]
        public void Methods_InvalidAlgorithm_ThrowsArgumentException(string invalidAlgo)
        {
            Assert.Throws<ArgumentException>(() => 
                _sut.ComputeChecksum(invalidAlgo, "text", out _, out _, out _));
        }

        #endregion
    }
}