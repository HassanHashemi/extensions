using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Extensions.hashtagUtility
{
    public class HashtagUtility
    {
        public List<string> TagExteraction(string input)
        {
            return Regex.Matches(input, @"\b?\#\w*\b")
                    .Cast<Match>()
                    .Select(i => i.Value)
                    .ToList();
        }
    }
}
