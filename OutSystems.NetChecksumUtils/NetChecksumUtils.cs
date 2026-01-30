using OutSystems.ExternalLibraries.SDK;
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace OutSystems.NetChecksumUtils
{
    /// <summary>
    /// Provides email validation utilities that wrap the EmailValidation library.
    /// </summary>
    public class NetChecksumUtils : INetChecksumUtils
    {
        static (string Hex, TimeSpan Elapsed) ComputeChecksum(Func<byte[], byte[]> hashFunc, byte[] bytes)
        {
            var sw = Stopwatch.StartNew();
            byte[] hashBytes = hashFunc(bytes);
            sw.Stop();
            return (Convert.ToHexString(hashBytes), sw.Elapsed);
        }

        /// <summary>
        /// Retorna a função de hashing correspondente ao nome do algoritmo.
        /// Lança ArgumentNullException se <paramref name="algorithm"/> for nulo e
        /// ArgumentException se o algoritmo não for suportado.
        /// </summary>
        /// <param name="algorithm">Nome do algoritmo (ex.: "SHA256", "SHA512", "SHA3-512", "MD5").</param>
        /// <returns>Função que recebe bytes e retorna o hash em bytes.</returns>
        private static Func<byte[], byte[]> GetHashFunction(string algorithm)
        {
            ArgumentNullException.ThrowIfNull(algorithm);

            return algorithm.Trim().ToUpperInvariant() switch
            {
                "SHA256" or "SHA-256" => SHA256.HashData,
                "SHA512" or "SHA-512" => SHA512.HashData,
                "MD5" => bytes =>
                {
                    using var md5 = MD5.Create();
                    return md5.ComputeHash(bytes);
                },
                "SHA3_256" or "SHA3-256" => SHA3_256.HashData,
                _ => throw new ArgumentException($"Unknown algorithm: {algorithm}", nameof(algorithm))
            };
        }

        /// <summary>
        /// Computes a hexadecimal checksum for the provided text using the specified algorithm, using the .Net System.Security.Cryptography dll.
        /// </summary>
        /// <param name="algorithm">
        /// The name of the hashing algorithm to use. Supported values (case-insensitive):
        /// "SHA256", "SHA512", "SHA3-256", "MD5".
        /// </param>
        /// <param name="textToHash">The text to compute the checksum for. The text is encoded as UTF-8 before hashing.</param>
        /// <param name="checksumText">Output parameter that receives the computed checksum as an uppercase hexadecimal string.</param>
        /// <param name="operationDurationInTicks">Output parameter that receives the duration of the hashing operation in ticks.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="algorithm"/> or <paramref name="textToHash"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="algorithm"/> is not recognized/supported.</exception>
        public void ComputeChecksum(
            string algorithm, string textToHash,
            out string checksumText, out long operationDurationInTicks)
        {
            // Ensure inputs are not null before proceeding
            ArgumentNullException.ThrowIfNull(algorithm);
            ArgumentNullException.ThrowIfNull(textToHash);

            // 1. Convert the input text into a byte array using UTF-8 encoding
            byte[] inputBytes = Encoding.UTF8.GetBytes(textToHash);

            // 2. Reuse a single method to obtain the hashing function
            var hashFunc = GetHashFunction(algorithm);

            // 3. Execute the hashing logic and measure performance.
            var (computedChecksum, elapsed) = ComputeChecksum(hashFunc, inputBytes);

            // 4. Assign results to output parameters
            checksumText = computedChecksum;
            operationDurationInTicks = elapsed.Ticks;
        }

        /// <summary>
        /// Verifies whether the checksum computed with the specified algorithm for the provided text matches the supplied existing checksum.
        /// </summary>
        /// <param name="algorithm">
        /// The name of the hashing algorithm to use. Supported values (case-insensitive):
        /// "SHA256", "SHA-256", "SHA512", "SHA-512", "MD5".
        /// </param>
        /// <param name="text">The input text to hash (UTF-8 encoded before hashing).</param>
        /// <param name="existingChecksum">The checksum to compare against (hex string).</param>
        /// <param name="isValid">Output parameter set to true when the computed checksum equals <paramref name="existingChecksum"/> (case-insensitive).</param>
        /// <param name="operationDuration">Output parameter that receives the duration, in ticks, of the entire verification operation (includes hashing + comparison).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="algorithm"/>, <paramref name="text"/> or <paramref name="existingChecksum"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="algorithm"/> is not recognized/supported.</exception>
        public void VerifyChecksum(
            string algorithm, string text, string existingChecksum,
            out bool isValid, out long operationDuration)
        {
            // Validate inputs
            ArgumentNullException.ThrowIfNull(algorithm);
            ArgumentNullException.ThrowIfNull(text);
            ArgumentNullException.ThrowIfNull(existingChecksum);

            // 1. Convert text to bytes (UTF-8)
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);

            // 2. Reuse the same method to obter o algoritmo de hash
            var hashFunc = GetHashFunction(algorithm);

            // 3. Measure the whole operation: hashing + comparison
            var sw = Stopwatch.StartNew();
            var (newChecksum, _) = ComputeChecksum(hashFunc, inputBytes);
            isValid = string.Equals(newChecksum, existingChecksum, StringComparison.OrdinalIgnoreCase);
            sw.Stop();

            // 4. Return operation duration in ticks (includes comparison step)
            operationDuration = sw.Elapsed.Ticks;
        }
    }
}