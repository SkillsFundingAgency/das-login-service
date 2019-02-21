using System;
using System.Security.Cryptography;
using System.Text;

namespace SFA.DAS.LoginService.Application.UnitTests.Helpers
{
    public static class HashExtensions
    {
        public static string GenerateHash(this string plaintext)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plaintext);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}