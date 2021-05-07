using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Interface
{
    public interface IHtmlRankingAnalyser
    {
        SearchEngine AnalyserType { get; }

        List<PageRank> ProcessHtml(string html);
    }
}
