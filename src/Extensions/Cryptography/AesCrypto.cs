using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tebyan.Extensions.Cryphtography
{
    /// <summary>
    /// Provides Cryptography methods based on AES cryptography implementation.
    /// </summary>
    public class AesCrypto
    {
        /// <summary>
        /// Gets or sets the salt to use with AES encryption.
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Gets or sets the passphrase for use with AES encryption.
        /// </summary>
        public string Passphrase { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="AesCrypto"/> from the specified values.
        /// </summary>
        /// <param name="salt">The <see cref="Salt"/> used in encryption.</param>
        /// <param name="passphrase">The <see cref="Passphrase"/> used in encryption.</param>
        public AesCrypto(byte[] salt, string passphrase)
        {
            if (string.IsNullOrWhiteSpace(passphrase))
            {
                throw new ArgumentException($"The parameter {nameof(passphrase)} is required.");
            }

            if (salt == null || salt.Length == 0)
            {
                throw new ArgumentException($"The parameter {nameof(salt)} is required.");
            }

            Salt = salt;
            Passphrase = passphrase;
        }

        /// <summary>
        /// Uses AES encryption to encrypt a string of data.
        /// </summary>
        /// <param name="clearText">The data to encrypt. Data is expected to be Unicode.</param>
        /// <returns>An encrypted Base64 string.</returns>
        public string Encrypt(string clearText)
        {
            if (string.IsNullOrEmpty(clearText))
            {
                throw new ArgumentNullException(nameof(clearText));
            }

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(Passphrase, Salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }

                    clearText = ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

            return this.Decrypt(cipherText, true);
        }

        /// <summary>
        /// Uses AES encryption to decrypt a string of data.
        /// </summary>
        /// <param name="cipherText">The encrypted Base64 string to decrypt.</param>
        /// <param name="Passphrase">The secret pre-shared passphrase.</param>
        /// <param name="throwExceptions">If true, will throw exceptions on decryption failure. Else, returns null string on decryption failure.</param>
        /// <returns>The plaintext data. Data is unicode.</returns>
        public string Decrypt(string cipherText, bool throwExceptions)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

            if (throwExceptions)
            {
                cipherText = AesDecryptInternal(cipherText);
            }
            else
            {
                try
                {
                    cipherText = AesDecryptInternal(cipherText);
                }
                catch
                {
                    cipherText = null;
                }
            }

            return cipherText;
        }

        private string AesDecryptInternal(string cipherText)
        {
            byte[] cipherBytes = FromBase64String(cipherText);

            using (var encryptor = Aes.Create())
            {
                var saltDerived = new Rfc2898DeriveBytes(Passphrase, Salt);
                encryptor.Key = saltDerived.GetBytes(32);
                encryptor.IV = saltDerived.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        static readonly char[] padding = { '=' };

        //https://stackoverflow.com/questions/26353710/how-to-achieve-base64-url-safe-encoding-in-c

        public static string ToBase64String(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd(padding)
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public static byte[] FromBase64String(string base64)
        {
            var incoming = base64
                .Replace('_', '/')
                .Replace('-', '+');

            switch (base64.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }

            return Convert.FromBase64String(incoming);
        }
    }
}
