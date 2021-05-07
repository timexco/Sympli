using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Interface
{
    public interface IPageFetcherSequenceResolver
    {
        SearchEngine SearchEngineType { get; }

        /// <summary>
        /// Determine the next sequence (paging) to fetch HTML contents.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="targetDate"></param>
        /// <returns>Next sequence to fetch content. Or null if to terminate fetch.</returns>
        Task<int?> Resolve(string keywords, DateTime targetDate);
    }
}
