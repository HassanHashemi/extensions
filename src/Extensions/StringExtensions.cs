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

namespace Project.Base
{
    public static class StringExtensions
    {
        /// <summary>
        /// set valid title for use in url that use '-' instead od spaceing  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUrlTitle(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return Regex.Replace(
                    Regex.Replace(
                        str.Trim(),
                        @"[^\w]{1,}", " ")
                        .Replace('"', '_').Trim('،'),
                    @"[\s\t\r\n]{1,}", "-");
        }

        public static int ToInt(this string integer, int defaultInt)
        {
            try
            {
                return int.Parse(integer);
            }
            catch
            {
                return defaultInt;
            }
        }

        public static string RSAEncryption(string DataStr, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] Data = Encoding.ASCII.GetBytes(DataStr);
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return Encoding.ASCII.GetString(encryptedData);
            }
            catch (CryptographicException e)
            {
                throw e;
            }
        }

        public static RSAParameters ToRSAParameter(this string Key)
        {
            var st = new StringReader(Key);
            var xs = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)xs.Deserialize(st);
        }

        public static string RSADecryption(string DataStr, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] Data = Encoding.ASCII.GetBytes(DataStr);
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return Encoding.ASCII.GetString(decryptedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
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
            if (String.IsNullOrEmpty(str))
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
        public static DateTime ToDateFromPersian(this string value, string defaultValue = "1300/01/01")
        {
            if (String.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            value = Regex.Replace(value, ".*?([0-9]{4}/[0-9]{2}/[0-9]{2}).*", "$1");
            var dateParts = value.Split(new[] { '/' }).Select(d => int.Parse(d)).ToArray();
            var hour = 0;
            var min = 0;
            var seconds = 0;
            DateTime date = new DateTime(dateParts[0], dateParts[1], dateParts[2], hour, min, seconds, new PersianCalendar());
            return date;
        }

        public static DateTime ToDate(this string date, DateTime defaultDateTime)
        {
            try
            {
                return DateTime.Parse(date.ToSqlDate());
            }
            catch
            {
                return defaultDateTime;
            }
        }

        public static DateTime SqlDateToDate(this string sqlDate, DateTime defaultDateTime)
        {
            try
            {
                return DateTime.Parse(sqlDate);
            }
            catch
            {
                return defaultDateTime;
            }
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
            catch (System.Exception e)
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

        public static string ToSqlDate(this string date)
        {
            string result = "NULL";
            var dd = "";
            var mm = "";
            var yyyy = "";
            if (date.Length > 0)
            {
                var buf = date;
                dd = buf.Substring(0, buf.IndexOf("-", 1, System.StringComparison.Ordinal));
                buf = buf.Substring(buf.IndexOf("-", 1, System.StringComparison.Ordinal) + 1, buf.Length - (buf.IndexOf("-", 1, System.StringComparison.Ordinal) + 1));
                mm = buf.Substring(0, buf.IndexOf("-", 1, System.StringComparison.Ordinal));
                buf = buf.Substring(buf.IndexOf("-", 1, System.StringComparison.Ordinal) + 1, buf.Length - (buf.IndexOf("-", 1, System.StringComparison.Ordinal) + 1));
                yyyy = buf;
                result = yyyy + "/" + mm + "/" + dd;
            }

            return result;
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
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static TResult Convert<TResult>(this string value)
        {
            return ConversionHelper.FromString<TResult>(value);
        }

        public static IEnumerable<TResult> Convert<TSource, TResult>(this IEnumerable<TSource> values)
        {
            return values.Select(s => s.ToString().Convert<TResult>());
        }

        public static byte[] ToByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        [Obsolete]
        public static string Left(this string value, int length)
        {
            return value.Left(length, false);
        }

        public static string Left(this string value, int length, bool completeWords)
        {
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                if (completeWords)
                {
                    return value.Substring(0, value.LastIndexOf(" "));
                }
                return value;
            }

            return value;
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static int[] ToIntArray(this string content)
        {
            List<int> arr = new List<int>();

            if (!String.IsNullOrEmpty(content))
            {
                string[] ints = content.Split(new char[] { ',' });

                foreach (string i in ints)
                {
                    arr.Add(System.Convert.ToInt32(i));
                }
            }

            return arr.ToArray();
        }

        public static string[] ToStringArray(this string content)
        {
            string[] arr = new string[] { };

            if (!String.IsNullOrEmpty(content))
            {
                arr = content.Split(new char[] { ',' }).ToArray();

            }

            return arr;
        }

        public static String StripHtmlTags(this String html)
        {
            if (String.IsNullOrEmpty(html))
            {
                return html;
            }
            else
            {
                return Regex.Replace(html, "<(.|\n)*?>", String.Empty).Replace("<", string.Empty);
            }
        }

        [Obsolete]
        public static String Shorten(this String html, int count)
        {
            return html.Shorten(count, false);
        }

        public static String Shorten(this String html, int count, bool completeWords)
        {
            if (String.IsNullOrEmpty(html) || count <= 0)
            {
                return html;
            }
            else
            {
                string noHtml = html.StripHtmlTags();
                if (noHtml.Count() > count)
                {
                    return String.Format("{0}...", noHtml.Left(count - 3, completeWords));

                }
                return noHtml;
            }
        }

        public static String UrlGuidEncode(this String content, string replaceBy)
        {
            string guid = content.RemoveSpecialCharacters(replaceBy).Replace(String.Format("{0}{0}", replaceBy), replaceBy);
            while (guid.StartsWith(replaceBy))
            {
                guid = guid.Substring(1);
            }

            while (guid.EndsWith(replaceBy))
            {
                guid = guid.Substring(0, guid.Length - 1);
            }

            return guid;
        }

        public static String RemoveSpecialCharacters(this String content, string replaceBy)
        {
            return Regex.Replace(content, "[^a-zA-Z0-9_.]+", replaceBy, RegexOptions.Compiled);
        }
    }
}
