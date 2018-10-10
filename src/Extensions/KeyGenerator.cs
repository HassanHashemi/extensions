using System;
using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public static class KeyGenerator
    {
        private static char[] _chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        /// <summary>
        /// Generate a random URL friendly string (youtube like)
        /// </summary>
        /// <param name="maxSize">result length</param>
        /// <returns>a random string</returns>
        public static string GetUniqueKey(int maxSize)
        {
            if (maxSize < 1)
            {
                throw new Exception("Length must be atleast 1");
            }

            var data = new byte[maxSize];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(_chars[b % _chars.Length]);
            }

            return result.ToString();
        }
    }
}
