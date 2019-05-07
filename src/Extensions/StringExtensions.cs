using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Extensions
{
    public static class StringExtensions
    {
        const string WINDOWS_DIR_SEPARATOR = @"\";
        const string LINUX_DIR_SEPARATOR = @"/";

        public static string ReplaceLinuxCharacter(string windowsPath)
        {
            return windowsPath.Replace(LINUX_DIR_SEPARATOR, WINDOWS_DIR_SEPARATOR);
        }

        public static string ReplaceWindowsCharacter(string windowsPath)
        {
            return windowsPath.Replace(WINDOWS_DIR_SEPARATOR, LINUX_DIR_SEPARATOR);
        }

        /// <summary>
        /// set valid title for use in url that use '-' instead od spaceing  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUrlTitle(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException();
            }
            return Regex.Replace(
                    Regex.Replace(
                        str.Trim(),
                        @"[^\w]{1,}", " ")
                        .Replace('"', '_').Trim('،'),
                    @"[\s\t\r\n]{1,}", "-");
        }

        public static string RSAEncryption(string data, RSAParameters key, bool addPadding)
        {
            try
            {
                byte[] Data = Encoding.ASCII.GetBytes(data);
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(key);
                    encryptedData = RSA.Encrypt(Data, addPadding);
                }
                return Encoding.ASCII.GetString(encryptedData);
            }
            catch (CryptographicException e)
            {
                throw e;
            }
        }

        public static RSAParameters ToRSAParameter(this string key)
        {
            var st = new StringReader(key);
            var xs = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)xs.Deserialize(st);
        }

        public static string RSADecryption(string DataStr, RSAParameters rsaKey, bool addPadding)
        {
            try
            {
                byte[] Data = Encoding.ASCII.GetBytes(DataStr);
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(rsaKey);
                    decryptedData = RSA.Decrypt(Data, addPadding);
                }
                return Encoding.ASCII.GetString(decryptedData);
            }
            catch (CryptographicException e)
            {
                throw e;
            }
        }

        public static string ToMd5Code(this string str)
        {
            byte[] hash;
            MD5 md = MD5CryptoServiceProvider.Create();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(str);
            hash = md.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                if (b > 16)
                {
                    sb.Append(b.ToString("x"));
                }
                else
                {
                    sb.Append("0");
                    sb.Append(b.ToString("x"));
                }
            }

            return (sb.ToString());
        }

        public static string GetHashString_MD5(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            byte[] hash;
            MD5 md = MD5CryptoServiceProvider.Create();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(str);
            hash = md.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b);
            }
            return (sb.ToString());
        }

        public static string Translate(this string key, Type resourceType)
        {
            try
            {
                ResourceManager rm = new ResourceManager(resourceType);
                var value = rm.GetString(key);
                return !string.IsNullOrWhiteSpace(value) ? value : key;
            }
            catch
            {
                return key;
            }
        }

        public static string Encrypt(this string stringToEncrypt)
        {
            try
            {
                StringBuilder StrB = new StringBuilder();
                StrB.Append("#");
                StrB.Append(stringToEncrypt);
                stringToEncrypt = StrB.ToString();
                string sEncryptionKey = "www.Tebyan.net";
                byte[] key = Encoding.UTF8.GetBytes(sEncryptionKey.ToCharArray(0, 8));
                byte[] IV = { 0x13, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return System.Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string Decrypt(this string stringToDecrypt)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {
                string sEncryptionKey = "www.Tebyan.net";
                byte[] key = Encoding.UTF8.GetBytes(sEncryptionKey.ToCharArray(0, 8));
                byte[] IV = { 0x13, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = System.Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                string retValue = encoding.GetString(ms.ToArray());
                if (retValue.Length > 0 && retValue[0] == '#')
                {
                    return retValue.Remove(0, 1);
                }

                return "-1";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string UrlEncode(string content)
        {
            string result = content.Replace(" ", "-");
            result = result.Replace("'", String.Empty);
            result = result.Replace(":", String.Empty);
            result = result.Replace("/", String.Empty);
            result = result.Replace("\\", String.Empty);
            result = result.Replace(".", String.Empty);
            result = result.Replace("?", String.Empty);
            result = result.Replace("&", String.Empty);
            result = result.Replace("--", "-");
            return result;
        }

        public static string UpperFirstCharacter(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return string.Format("{0}{1}", text.Substring(0, 1).ToUpper(), text.Substring(1).ToLower());
            }
            return text;
        }

        public static string ReverseString(this string value)
        {
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static byte[] ToByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string Left(this string value, int length, bool completeWords)
        {
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                var lastindex = value.LastIndexOf(" ");
                if (completeWords && lastindex != -1)
                {
                    return value.Substring(0, lastindex);
                }
                return value;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static int[] ToIntArray(this string content)
        {
            var array = new List<int>();
            if (!string.IsNullOrEmpty(content))
            {
                string[] ints = content.Split(new char[] { ',' });
                foreach (string i in ints)
                {
                    try
                    {
                        array.Add(Convert.ToInt32(i));
                    }
                    catch
                    {
                        throw new FormatException();
                    }
                }
                return array.ToArray();
            }
            throw new ArgumentNullException();
        }

        public static string[] ToStringArray(this string content)
        {
            throw new NotImplementedException();
            //var arr = new string[] { };
            //if (!string.IsNullOrEmpty(content))
            //{
            //    arr = content.Split(new char[] { ',' }).ToArray();
            //    return arr;
            //}
            //throw new ArgumentNullException();
        }

        public static string StripHtmlTags(this string html)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrEmpty(html))
            //{
            //    return html;
            //}
            //else
            //{
            //    return Regex.Replace(html, "<(.|\n)*?>", string.Empty).Replace("<", string.Empty);
            //}
        }


        public static string RemoveSpecialCharacters(this string content, string replaceBy)
        {
            return Regex.Replace(content, "[^a-zA-Z0-9_.]+", replaceBy, RegexOptions.Compiled);
        }
    }
}
