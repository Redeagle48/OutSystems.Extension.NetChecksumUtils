using System;
using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.NetChecksumUtils
{
    /// <summary>
    /// The checksum result structure containing both Hex and Base64 representations.
    /// </summary>
    [OSStructure(
        Description = "Represents a computed checksum in both hexadecimal and Base64 encodings.",
        OriginalName = "Checksum"
    )]
    public struct Checksum
    {
        /// <summary>
        /// Gets the hexadecimal representation of the checksum.
        /// </summary>
        [OSStructureField(
            Description = "Hexadecimal representation of the checksum.",
            OriginalName = "Hex"
        )]
        public string Hex;

        /// <summary>
        /// Gets the Base64 representation of the checksum.
        /// </summary>
        [OSStructureField(
            Description = "Base64 representation of the checksum.",
            OriginalName = "Base64"
        )]
        public string Base64;
    }
}