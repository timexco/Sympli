using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sympli.SearchRankingAnalyser.Helpers;

namespace Sympli.SearchRankingAnalyser.Google
{
    public class GoogleHtmlRankingAnalyser : IHtmlRankingAnalyser
    {
        public SearchEngine AnalyserType => SearchEngine.Google;

        public virtual List<PageRank> ProcessHtml(string html)
        {
            //matches anything that similar to "/url?q=htts://somesiteurl?qstr=blahblahblah"
            var rx = new Regex(@"(?i)""/url\?q=(?<text>.*?)""", RegexOptions.Singleline);
            var matches = rx.Matches(html);

            int i = 1;

            return matches
                        .OrderBy(c => c.Index)
                        .Where(c => !c.Value.ContainsAny(ExclusionUrls)) //be careful, we need to exclude some links from google
                        .Select(c => new PageRank
                        {
                            Rank = i++,
                            Url = c.Groups[1].Value
                        }).ToList();
        }

        private List<string> ExclusionUrls = new List<string> { "google.com" };


    }
}
