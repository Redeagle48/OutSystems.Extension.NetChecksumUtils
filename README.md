![Tests Status](https://github.com/Redeagle48/OutSystems.Extension.NetChecksumUtils/actions/workflows/test.yml/badge.svg) [![codecov](https://codecov.io/github/Redeagle48/OutSystems.Extension.NetChecksumUtils/graph/badge.svg?token=KFNT6WP1VY)](https://codecov.io/github/Redeagle48/OutSystems.Extension.NetChecksumUtils)
# OutSystems.Extension.NetChecksumUtils
A high-performance OutSystems Extension using the .Net System.Security.Cryptography DLL to provide robust checksum generation and verification for strings and binary data. This extension simplifies data integrity checks by wrapping standard .NET cryptographic providers into easy-to-use Server Actions. This implementation replaces the obsolete "ComputeHash" OutSystems built-in action. 

🚀 Features
Multi-Algorithm Support: Generate hashes using MD5, SHA256, SHA3_256, and SHA512.

## Hash Algorithms available
| **Algorithm** | **Performance (Speed)** | **Security Level** | **Best OutSystems Use Case**                                             |
|---------------|-------------------------|--------------------|--------------------------------------------------------------------------|
| MD5           | Fastest                 | Broken             | Quick file integrity (e.g. "Did this 2KB text file download correctly?") |
| SHA-256       | Moderate                | High               | The "Standard". Use for API integrations and standard hashing.           |
| SHA-512       | Fast (on 64-bit)        | High               | High-performance hashing for large binaries/blobs.                       |
| SHA3-512      | Slower (Software)       | Maximum            | Highly sensitive data; resistance against Length Extension Attacks.      |


🛠 Actions Included
This extension exposes the following Server Actions within Service Studio:

## ComputeHash
Generates a checksum for a given input based on the specified algorithm.

Input: Value (Text/Binary), Algorithm (Text: MD5, SHA256, etc.)

Output: Hash (Text)

## VerifyHash
Compares a plain value against an existing hash to check for integrity.

Input: Value (Text/Binary), HashToVerify (Text), Algorithm (Text: MD5, SHA256, etc.)

Output: IsValid (Boolean)