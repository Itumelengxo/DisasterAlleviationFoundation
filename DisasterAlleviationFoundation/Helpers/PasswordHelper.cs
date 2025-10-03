using System;
using System.Security.Cryptography;
using System.Text;

namespace DisasterAlleviationFoundation.Helpers
{
    public static class PasswordHelper
    {
        // Return tuple (hash, salt) as base64 strings
        public static (string hash, string salt) HashPassword(string password, int iterations = 100_000)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            using var derive = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256);
            var hashBytes = derive.GetBytes(32);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        public static bool VerifyPassword(string password, string storedHashBase64, string storedSaltBase64, int iterations = 100_000)
        {
            if (string.IsNullOrEmpty(storedHashBase64) || string.IsNullOrEmpty(storedSaltBase64))
                return false;

            var saltBytes = Convert.FromBase64String(storedSaltBase64);
            using var derive = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256);
            var testHash = derive.GetBytes(32);
            var storedHash = Convert.FromBase64String(storedHashBase64);
            return CryptographicOperations.FixedTimeEquals(storedHash, testHash);
        }
    }
}
