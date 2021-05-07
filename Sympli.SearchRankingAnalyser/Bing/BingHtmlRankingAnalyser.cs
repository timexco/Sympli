using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Bing
{
    public class BingHtmlRankingAnalyser : IHtmlRankingAnalyser
    {
        public SearchEngine AnalyserType => SearchEngine.Bing;

        public virtual List<PageRank> ProcessHtml(string html)
        {
            throw new NotImplementedException();
        }
    }
}
