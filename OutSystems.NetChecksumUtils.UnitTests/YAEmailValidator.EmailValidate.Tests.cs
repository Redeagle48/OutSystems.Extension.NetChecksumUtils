using NUnit.Framework;
using System;
using System.Text;
using System.Security.Cryptography;

namespace OutSystems.NetChecksumUtils.Tests
{
    [TestFixture]
    public class ComputeChecksumTests
    {/*
        private readonly NetChecksumUtils _sut = new();

        private static string ComputeExpected(string algorithm, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            return algorithm.Trim().ToUpperInvariant() switch
            {
                "SHA256" or "SHA-256" => Convert.ToHexString(SHA256.HashData(data)),
                "SHA512" or "SHA-512" => Convert.ToHexString(SHA512.HashData(data)),
                "MD5" => Convert.ToHexString(MD5.Create().ComputeHash(data)),
                _ => throw new ArgumentException("Unsupported algorithm", nameof(algorithm))
            };
        }

        [TestCase("SHA256")]
        [TestCase("SHA-256")]
        [TestCase("sha256")]
        public void ComputeChecksum_SHA256_Variants_ReturnsExpected(string algorithm)
        {
            string text = "hello";
            string expected = ComputeExpected(algorithm, text);

            _sut.ComputeChecksum(algorithm, text, out var actual, out var ticks);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(ticks, Is.GreaterThan(0));
        }

        [TestCase("SHA512")]
        [TestCase("SHA-512")]
        [TestCase("sha512")]
        public void ComputeChecksum_SHA512_Variants_ReturnsExpected(string algorithm)
        {
            string text = "the quick brown fox";
            string expected = ComputeExpected(algorithm, text);

            _sut.ComputeChecksum(algorithm, text, out var actual, out var ticks);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(ticks, Is.GreaterThan(0));
        }

        [TestCase("MD5")]
        [TestCase("md5")]
        public void ComputeChecksum_MD5_ReturnsExpected(string algorithm)
        {
            string text = "some text for md5";
            string expected = ComputeExpected(algorithm, text);

            _sut.ComputeChecksum(algorithm, text, out var actual, out var ticks);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(ticks, Is.GreaterThan(0));
        }

        [Test]
        public void ComputeChecksum_InvalidAlgorithm_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _sut.ComputeChecksum("UNKNOWN", "text", out _, out _));
        }

        [Test]
        public void ComputeChecksum_NullAlgorithm_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _sut.ComputeChecksum(null!, "text", out _, out _));
        }

        [Test]
        public void ComputeChecksum_NullText_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _sut.ComputeChecksum("SHA256", null!, out _, out _));
        }
        */
    }
}