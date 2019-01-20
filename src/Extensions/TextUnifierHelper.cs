using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class TextUnifierHelper
    {
        private static readonly TimeSpan _defaultTimeOut = TimeSpan.FromMilliseconds(300);

        private static readonly Dictionary<string, string> ReplacementMap = new Dictionary<string, string>()
        {
            { "[آأإ]", "ا" },
            { "[ي]", "ی" },
            { "[ة]", "ه" },
            { "[ؤ]", "و" },
            { "[ك]", "ک" }
        };

        public static string RemoveDiacriticsAndNotAlpha(this string source)
        {
            return Replace(source, @"[ًٌٍَُِّـ\W\s،؛,ء_]", string.Empty).ToLower();
        }

        public static string ReplaceArabicCharacters(this string source)
        {
            foreach (var arabicPattern in ReplacementMap.Keys)
            {
                source = Replace(source, arabicPattern, ReplacementMap[arabicPattern]);
            }

            return source.ToLower();
        }

        private static string Replace(string input, string pattern, string value) =>
            Regex.Replace(input, pattern, value, RegexOptions.None, _defaultTimeOut);
    }
}
