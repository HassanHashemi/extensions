using System;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class RegexHelpers
    {
        public static bool Match(string input, string pattern)
        {
            return Match(input, pattern, TimeSpan.FromMilliseconds(300));
        }

        public static bool Match(string input, string pattern, TimeSpan timeout)
        {
            return Regex.IsMatch(input, pattern, default, timeout);
        }
    }
}
