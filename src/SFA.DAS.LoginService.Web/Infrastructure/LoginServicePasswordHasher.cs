using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    /// <summary>
    /// A replacement for the standard Identity Server V4 hasher to be backwards compatible with existing das-employeruser hashes;
    /// when a das-employeruser password has been verified it will be replaced with a standard Identity Server V4 hash
    /// </summary>
    public class LoginServicePasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
    {
        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) { throw new ArgumentNullException(nameof(hashedPassword)); }
            if (providedPassword == null) { throw new ArgumentNullException(nameof(providedPassword)); }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }

            // the current ASP.NET Core format providers are 0x00 and 0x01, the transferred das-employerusers hashes will be given the highest marker
            // as that will be good for the foreseeable future if the markers are being incremented periodically.
            if (decodedHashedPassword[0] == 0xFF)
            {
                // skipping the format code, split the hashed password into the first 16 characters for the salt and the remainder for the password hash
                byte[] salt = decodedHashedPassword.Skip(1).Take(16).ToArray();
                string storedPasswordHash = Convert.ToBase64String(decodedHashedPassword.Skip(1).Skip(16).ToArray());

                if (VerifyHashedPasswordEmployerUsers(salt, storedPasswordHash, providedPassword))
                {
                    // correctly verified a das-employerusers password hash format - the caller needs to rehash
                    return PasswordVerificationResult.SuccessRehashNeeded;
                }
                else
                {
                    return PasswordVerificationResult.Failed;
                }
            }

            // verification of current Identity Server V4 hashes by default
            return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }

        /// <summary>
        /// Verifies the hashed password for a das-employerusers user.
        /// </summary>
        /// <param name="salt">The salt for the original hash.</param>
        /// <param name="storedPasswordHash">The stored password hash.</param>
        /// <param name="password">The password plain text.</param>
        /// <returns></returns>
        private bool VerifyHashedPasswordEmployerUsers(byte[] salt, string storedPasswordHash, string password)
        {
            if (salt.Length < 16 || storedPasswordHash.Length < 2)
            {
                return false; // bad size
            }

            // using hardcoded password profile data from das-employerusers application; as there was no unique
            // password profiles stored in the orginal database
            string keyData = Convert.ToBase64String(Encoding.ASCII.GetBytes("DZUvwHBMEdtGMi6CC@tRrFrcj7sJx["));
            int workFactor = 10000;
            int storageLength = 256;

            // using the original salt and the plain text password
            var saltedPassword = salt.Concat(Encoding.Unicode.GetBytes(password)).ToArray();

            var hasher = new HMACSHA256(Convert.FromBase64String(keyData));
            var hash = hasher.ComputeHash(saltedPassword);

            // generate a hashed password which can be compared to the stored password hash
            var pbkdf2 = new Rfc2898DeriveBytes(Convert.ToBase64String(hash), salt, workFactor);
            var generatedPasswordHash = pbkdf2.GetBytes(storageLength);

            // the password is verified if the generated hash matches the stored original hash
            return storedPasswordHash.Equals(Convert.ToBase64String(generatedPasswordHash));
        }
    }
}
