using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.NetChecksumUtils
{
    /// <summary>
    /// Computes a hexadecimal checksum for the provided text using the specified algorithm (SHA256, SHA512, MD5, SHA3_256, using the .Net System.Security.Cryptography dll.
    /// </summary>
    [OSInterface(
        Description = "Computes a hexadecimal checksum for the provided text using the specified algorithm (SHA256, SHA512, MD5, SHA3_256, using the .Net System.Security.Cryptography dll.",
        IconResourceName = "OutSystems.NetChecksumUtils.resources.NetChecksumUtils_icon.png"
    )]
    public interface INetChecksumUtils
    {
        [OSAction(
            Description = "Computes a hexadecimal checksum for the provided text using the specified algorithm.",
            IconResourceName = "OutSystems.NetChecksumUtils.resources.NetChecksumUtils_icon.png"
        )]
        void ComputeChecksum(
            [OSParameterAttribute(Description = "The name of the hashing algorithm to use. Supported values (case-insensitive): \"SHA256\", \"SHA-256\", \"SHA512\", \"SHA-512\", \"MD5\", \"SHA3-256\", \"SHA3_256\".")]
            string algorithm,
            [OSParameterAttribute(Description = "The text to compute the checksum for. The text is encoded as UTF-8 before hashing.")]
            string textToHash,
            [OSParameterAttribute(Description = "Output parameter that receives the computed checksum as an uppercase hexadecimal string.")]
            out string checksumText,
            [OSParameterAttribute(Description = "Output parameter that receives the computed checksum as a Base64 string.")]
            out string checksumBase64,
            [OSParameterAttribute(Description = "Output parameter that receives the duration of the hashing operation in ticks.")]
            out long operationDuration
        );

        [OSAction(
            Description = "Computes a hexadecimal checksum for the provided text using the specified algorithm.",
            IconResourceName = "OutSystems.NetChecksumUtils.resources.NetChecksumUtils_icon.png"
        )]
        void VerifyChecksum(
            [OSParameterAttribute(Description = "The name of the hashing algorithm to use. Supported values (case-insensitive): \"SHA256\", \"SHA-256\", \"SHA512\", \"SHA-512\", \"MD5\", \"SHA3-256\", \"SHA3_256\".")]
            string algorithm,
            [OSParameterAttribute(Description = "The name of the hashing algorithm to use. Supported values (case-insensitive): \"SHA256\", \"SHA-256\", \"SHA512\", \"SHA-512\", \"MD5\", \"SHA3-256\", \"SHA3_256\".")]
            string text,
            [OSParameterAttribute(Description = "The text to compute the checksum for uppercase hex or base64). The text is encoded as UTF-8 before hashing.")]
            string existingChecksum,
            [OSParameterAttribute(Description = "Output parameter that receives the computed checksum as an uppercase hexadecimal string.")]
            out bool isValid,
            [OSParameterAttribute(Description = "Output parameter that receives the duration of the hashing operation in ticks.")]
            out long operationDuration
        );
    }
}
