using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Bing
{
    public class BingPageFetcherRouteResolver : IPageFetcherRouteResolver
    {
        public SearchEngine SearchEngineType => SearchEngine.Bing;

        public virtual string Resolve(int sequence, string keywords)
        {
            throw new NotImplementedException();
        }
    }
}
