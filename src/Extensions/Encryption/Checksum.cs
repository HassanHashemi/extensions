using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Encryption
{
    public static class Checksum
    {
        public const int BLOCK_SIZE = 8 * 1024;

        public static Task<string> GetMd5Async(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Stream is not readable.");
            }

            return GetHashAsync<MD5CryptoServiceProvider>(stream);
        }

        private static async Task<string> GetHashAsync<T>(this Stream stream) where T : HashAlgorithm, new()
        {
            StringBuilder sb;

            using (var algo = new T())
            {
                var buffer = new byte[BLOCK_SIZE];
                int read;

                while (( read = await stream.ReadAsync(buffer, 0, buffer.Length) ) == buffer.Length)
                {
                    algo.TransformBlock(buffer, 0, read, buffer, 0);
                }

                algo.TransformFinalBlock(buffer, 0, read);

                sb = new StringBuilder(algo.HashSize / 4);
                foreach (var b in algo.Hash)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
            }

            return sb?.ToString();
        }
    }
}
