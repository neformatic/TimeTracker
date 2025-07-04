using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using TimeTracker.BLL.Helpers.Interfaces;

namespace TimeTracker.BLL.Helpers;

public class HashHelper : IHashHelper
{
    private const int SaltSize = 128 / 8;

    public string GenerateHash(string password)
    {
        const int pbkdf2Iterations = 1000;
        const int numBytesRequested = 256 / 8;
        const KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA512;

        var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        var subKey = KeyDerivation.Pbkdf2(password, salt, prf, pbkdf2Iterations, numBytesRequested);
        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        outputBytes[0] = 0x01; // format marker

        WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
        WriteNetworkByteOrder(outputBytes, 5, pbkdf2Iterations);
        WriteNetworkByteOrder(outputBytes, 9, SaltSize);

        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + SaltSize, subKey.Length);

        return Convert.ToBase64String(outputBytes);
    }

    public bool VerifyHash(string hashedPassword, string password)
    {
        var hashedPasswordBase64 = Convert.FromBase64String(hashedPassword);

        try
        {
            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPasswordBase64, 1);
            var iterCount = (int)ReadNetworkByteOrder(hashedPasswordBase64, 5);
            var saltLength = (int)ReadNetworkByteOrder(hashedPasswordBase64, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < SaltSize)
            {
                return false;
            }

            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPasswordBase64, 13, salt, 0, salt.Length);

            // Read the sub key (the rest of the payload): must be >= 128 bits
            var subKeyLength = hashedPasswordBase64.Length - 13 - salt.Length;
            if (subKeyLength < SaltSize)
            {
                return false;
            }

            var expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashedPasswordBase64, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            // Hash the incoming password and verify it
            var actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subKeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        }
        catch
        {
            // This should never occur except in the case of a malformed payload, where
            // we might go off the end of the array. Regardless, a malformed payload
            // implies verification failed.
            return false;
        }
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }

    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
               | ((uint)(buffer[offset + 1]) << 16)
               | ((uint)(buffer[offset + 2]) << 8)
               | buffer[offset + 3];
    }
}