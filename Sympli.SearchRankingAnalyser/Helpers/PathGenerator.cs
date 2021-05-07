using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Helpers
{
    public static class PathGenerator
    {
        public static string GenerateStoragePath(SearchEngine searchEngine, string keywords, DateTime date, int sequence)
        {
            return $"SearchRankings/{searchEngine.ToString()}/{keywords}/{String.Format("{0:yyyy-MM-dd}", date)}/{sequence}.html";
        }

        public static string Generate(SearchEngine searchEngine, string keywords, DateTime date)
        {
            return $"SearchRankings/{searchEngine.ToString()}/{keywords}/{String.Format("{0:yyyy-MM-dd}", date)}/";
        }
    }
}
