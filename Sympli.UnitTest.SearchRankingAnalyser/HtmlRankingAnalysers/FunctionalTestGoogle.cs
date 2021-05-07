using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sympli.SearchRankingAnalyser.Google;
using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sympli.UnitTest.SearchRankingAnalyser.HtmlRankingAnalysers
{
    [TestClass]
    public class FunctionalTestGoogle
    {
        [TestMethod]
        public void Google_ProcessHtml_ExtractPageRank_Success_Single()
        {
            string html = @"
<div>
<a href=""/url?q=https://www.sympli.com.au"">Some Link 1</a>
</div>
";
            List<PageRank> expected = new List<PageRank> { new PageRank { Rank = 1, Url = "https://www.sympli.com.au" } };

            IHtmlRankingAnalyser analyser = new GoogleHtmlRankingAnalyser();
            List<PageRank> actual = analyser.ProcessHtml(html);

            CollectionAssert.AreEqual(expected, actual, new PageRankComparer());
        }

        [TestMethod]
        public void Google_ProcessHtml_ExtractPageRank_Success_Multiple()
        {
            string html = @"
        <div>
        <a href=""/url?q=https://www.sympli.com.au"">Some Link 2</a>
        <a href=""/url?q=https://trinitylaw.com.au/100-econveyancing-in-nsw-electronic-settlements/"">Some Link 3</a>
        <a href=""/url?q=http://nationalconveyancingsolutions.com.au/e-settlements.html"">Some Link 4</a>
        </div>
        ";
            List<PageRank> expected = new List<PageRank> { 
                new PageRank { Rank = 1, Url = "https://www.sympli.com.au" },
                new PageRank { Rank = 2, Url = "https://trinitylaw.com.au/100-econveyancing-in-nsw-electronic-settlements/" },
                new PageRank { Rank = 3, Url = "http://nationalconveyancingsolutions.com.au/e-settlements.html" }
            };

            IHtmlRankingAnalyser analyser = new GoogleHtmlRankingAnalyser();
            List<PageRank> actual = analyser.ProcessHtml(html);

            CollectionAssert.AreEqual(expected, actual, new PageRankComparer());
        }

        [TestMethod]
        public void Google_ProcessHtml_ExtractPageRank_ExcludeGoogle_Success_Multiple()
        {
            string html = @"
        <div>
        <a href=""/url?q=https://www.sympli.com.au"">Some Link 2</a>
        <a href=""/url?q=https://trinitylaw.com.au/100-econveyancing-in-nsw-electronic-settlements/"">Some Link 3</a>
        <a href=""/url?q=http://nationalconveyancingsolutions.com.au/e-settlements.html"">Some Link 4</a>
        <a href=""/url?q=http://www.google.com.au"">Some Link 5</a>
        <a href=""/url?q=https://photos.google.com/"">Some Link 6</a>
        </div>
        ";
            List<PageRank> expected = new List<PageRank> {
                new PageRank { Rank = 1, Url = "https://www.sympli.com.au" },
                new PageRank { Rank = 2, Url = "https://trinitylaw.com.au/100-econveyancing-in-nsw-electronic-settlements/" },
                new PageRank { Rank = 3, Url = "http://nationalconveyancingsolutions.com.au/e-settlements.html" }
            };

            IHtmlRankingAnalyser analyser = new GoogleHtmlRankingAnalyser();
            List<PageRank> actual = analyser.ProcessHtml(html);

            CollectionAssert.AreEqual(expected, actual, new PageRankComparer());
        }

        /// <summary>
        /// Perform instant fetch and process to ensure the page/element structure has not been updated by Google
        /// </summary>
        [TestMethod]
        public async Task Google_ProcessHtml_ExtractPageRank_FormatVerify_Success()
        {
            string targetUrl = "https://www.google.com.au/search?q=e-settlements";

            HttpClient client = new HttpClient();
            HttpRequestMessage fetchRequest = new HttpRequestMessage(HttpMethod.Get, targetUrl);

            HttpResponseMessage response = await client.SendAsync(fetchRequest);
            string htmlContent = await response.Content.ReadAsStringAsync();

            IHtmlRankingAnalyser analyser = new GoogleHtmlRankingAnalyser();
            List<PageRank> actual = analyser.ProcessHtml(htmlContent);

            //ensure there are results
            Assert.IsTrue(actual.Count > 0, "No resulting PageRank could be found");

        }


        private class PageRankComparer : IComparer, IComparer<PageRank>
        {
            public int Compare(PageRank x, PageRank y)
            {
                if (x.Rank == y.Rank && x.Url == y.Url)
                    return 0;

                return -1;
            }

            public int Compare(object x, object y)
            {
                return Compare(x as PageRank, y as PageRank);
            }
        }
    }
}
