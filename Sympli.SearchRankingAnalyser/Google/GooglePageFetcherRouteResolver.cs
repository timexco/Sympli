using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Google
{
    public class GooglePageFetcherRouteResolver : IPageFetcherRouteResolver
    {
        public SearchEngine SearchEngineType => SearchEngine.Google;

        public virtual string Resolve(int sequence, string keywords)
        {
            return $"https://www.google.com.au/search?q={keywords}&start={(sequence - 1) * 10}";
        }
    }
}
