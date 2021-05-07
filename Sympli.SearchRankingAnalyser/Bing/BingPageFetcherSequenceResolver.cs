using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Google
{
    public class BingPageFetcherSequenceResolver : IPageFetcherSequenceResolver
    {
        public SearchEngine SearchEngineType => SearchEngine.Bing;

        public async Task<int?> Resolve(string keywords, DateTime targetDate)
        {
            throw new NotImplementedException();
        }
    }
}
