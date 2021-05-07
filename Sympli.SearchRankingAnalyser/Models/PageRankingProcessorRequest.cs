using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sympli.SearchRankingAnalyser.Models
{
    public class PageRankingProcessorRequest
    {
        [FromQuery]
        public SearchEngine SearchEngine { get; set; }
        [FromQuery]
        public string Keywords { get; set; }
        [FromQuery]
        public DateTime TargetDate { get; set; } = DateTime.Now;
        [FromQuery]
        public string Match { get; set; }
        [FromQuery]
        public bool ShowDetails { get; set; } = false;
    }

    public class PageRankingProcessorResponse
    {
        public DateTime Date { get; set; }
        public List<PageRank> Pages { get; set; }
    }

    public class PageRankingProcessorWithSequenceResponse : PageRankingProcessorResponse
    {
        public int Sequence { get; set; }
    }



    public class PageRank
    {
        public int Rank { get; set; }
        public string Url { get; set; }
    }

}
