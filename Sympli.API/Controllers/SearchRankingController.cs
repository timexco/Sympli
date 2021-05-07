using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sympli.SearchRankingAnalyser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sympli.SearchRankingAnalyser.Models;
using Newtonsoft.Json;

namespace Sympli.API.Controllers
{
    /// <summary>
    /// A simple API endpoint to demostrate the search ranking feature
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SearchRankingController : ControllerBase
    {
        private readonly PageFetcher _pageFetcher;
        private readonly PageRankingProcessor _processor;

        public SearchRankingController(PageFetcher fetcher, PageRankingProcessor processor)
        {
            _pageFetcher = fetcher;
            _processor = processor;
        }

        [HttpGet]
        [Route("Fetch")]
        public async Task<IActionResult> Fetch([FromQuery] PageFetcherRequest request)
        {
            PageFetcherResponse response = await _pageFetcher.PageFetcherHandler(request);
            if (response.Success)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("Results")]
        public async Task<string> Results([FromQuery] PageRankingProcessorRequest request) //SearchEngine searchEngine, DateTime? date, string keywords, string match, bool showDetails = false)
        {
            PageRankingProcessorResponse pTask = await _processor.PageRankingProcessorHandler(request);

            if (pTask?.Pages?.Count < 100)
                return "Results unavailable";

            IEnumerable<PageRank> matchingRanks = pTask.Pages.Take(100); //only take the first 100

            if (!string.IsNullOrEmpty(request.Match))
                matchingRanks = matchingRanks.Where(c => c.Url.ToUpper().Contains(request.Match.ToUpper())); //ranks that matches requested

            if (request.ShowDetails)
                return JsonConvert.SerializeObject(matchingRanks);

            return String.Join(",", matchingRanks.Select(c => c.Rank)); //show rank only
        }

    }
}
