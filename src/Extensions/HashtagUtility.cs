using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class HashtagUtility
    {
        public static string ReplaceHashtag(string input, Func<string, string> evlauator)
        {
            return Regex.Replace(input, @"\b?\#\w*\b", match => evlauator(match.Value));
        }

        public static IEnumerable<string> TagExteraction(string input)
        {
            var timeout = new TimeSpan(0, 0, 0, 0, 100);
            return Regex.Matches(input, @"\b?\#\w*\b", RegexOptions.None, timeout)
                    .Cast<Match>()
                    .Select(i => Regex.Replace(i.Value, @"\#", string.Empty))
                    .ToList();
        }
    }
}
