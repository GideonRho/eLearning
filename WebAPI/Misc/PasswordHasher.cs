using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebAPI.Misc
{
    public class PasswordHasher
    {
        
        public byte[] Salt { get; private set; }

        public PasswordHasher(byte[] salt)
        {
            Salt = salt;
        }

        public PasswordHasher()
        {
            Salt = GenerateSalt();
        }

        public string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
        }
        
        private byte[] GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[128 / 8];
            rng.GetBytes(salt);
            return salt;
        }
        
    }
}