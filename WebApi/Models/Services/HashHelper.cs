﻿using System.Security.Cryptography;
using System.Text;

namespace WebApi.Models.Services
{
    public static class HashHelper
    {
        public static string ComputeHash(string password, string salt, string pepper, int iterations)
        {
            if (iterations <= 0) return password;

            using var hmac = SHA256.Create();

            var passwordSaltPepper = $"{password}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = hmac.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return ComputeHash(hash, salt, pepper, iterations - 1);
        }

        public static string GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            return Convert.ToBase64String(byteSalt);
        }
    }
}
