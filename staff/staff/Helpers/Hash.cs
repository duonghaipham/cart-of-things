using System;
using System.Security.Cryptography;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace staff.Helpers
{
    public class Hash
    {
        private static Hash _instance;
        private Hash() { }

        public static Hash GetInstance()
        {
            if (_instance == null)
                _instance = new Hash();
            return _instance;
        }

        public string GetHash(string input)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = sha256.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }

        public bool VerifyHash(string input, string hash)
        {
            string hashOfInput = GetHash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
                return true;

            return false;
        }
    }
}