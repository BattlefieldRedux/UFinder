using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UFinder
{
    public class Pool
    {
        public List<string> Find { get; set; }
        public List<string> Replace { get; set; }


        public Pool() {
            Find = new List<string>();
            Replace = new List<string>();
        }


        internal string FindAndReplace(string line, bool ignoreCase)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string pattern = string.Join("|", Find);

            return Regex.Replace(line, pattern, (match) =>
            {
                int rn = rnd.Next(0, Replace.Count);
                return Replace[rn];
            }, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
    }
}
