using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sympli.SearchRankingAnalyser.Models
{
    public class PageFetcherRequest
    {
        [FromQuery]
        public string SearchKeywords { get; set; }
        //[FromQuery]
        //public int Sequence { get; set; }
        [FromQuery]
        public SearchEngine SearchEngineType { get; set; }
    }

    public class PageFetcherResponse
    {
        public string FilePath { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public enum SearchEngine
    {
        Google,
        Bing
    }

}
