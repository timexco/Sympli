using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Helpers
{
    public static class StringHelper
    {
        public static bool ContainsAny(this string haystack, List<string> needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
    }
}
