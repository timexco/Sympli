using Sympli.Common;
using Sympli.Common.Models.StorageClient;
using Sympli.SearchRankingAnalyser.Helpers;
using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser
{
    public class PageFetcher
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStorageClient _storageClient;
        private readonly IEnumerable<IPageFetcherRouteResolver> _routeResolvers;
        private readonly IEnumerable<IPageFetcherSequenceResolver> _sequenceResolvers;
        public PageFetcher(IStorageClient storageClient, IHttpClientFactory httpClientFactory, IEnumerable<IPageFetcherRouteResolver> routeResolvers, IEnumerable<IPageFetcherSequenceResolver> sequenceResolvers)
        {
            _storageClient = storageClient;
            _httpClientFactory = httpClientFactory;
            _routeResolvers = routeResolvers;
            _sequenceResolvers = sequenceResolvers;
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="request">Information for the page request to be fetched</param>
        public async Task<PageFetcherResponse> PageFetcherHandler(PageFetcherRequest request)
        {
            //validate inputs, throw argument exception and terminate function if failed
            ValidateRequest(request);

            int? sequence = await ResolveSequence(request.SearchEngineType, request.SearchKeywords); //if none found than we fetch the first sequence

            if (sequence == null)
                return new PageFetcherResponse { Success = true, Message = $"Max sequence/paging has been achieved for the search of {request.SearchKeywords}" };

            string resolvedTargetRoute = ResolveRoute(request.SearchEngineType, request.SearchKeywords, sequence.Value);

            //fetch the html content of the target url
            string html = await FetchHtml(resolvedTargetRoute);

            //store it on storage
            string fileName = PathGenerator.GenerateStoragePath(request.SearchEngineType, request.SearchKeywords, DateTime.Now, sequence.Value);
            StorageClientUploadResponse uploadResponse = await StoreHtmlToStorage(fileName,html);

            return new PageFetcherResponse { Success = true, FilePath = fileName };
        }

        private async Task<int?> ResolveSequence(SearchEngine searchEngineType, string searchKeywords)
        {
            IPageFetcherSequenceResolver resolver = _sequenceResolvers.Where(c => c.SearchEngineType == searchEngineType).FirstOrDefault();
            if (resolver == null)
                throw new Exception("Sequence resolver not available for search engine: " + searchEngineType.ToString());

            return await resolver.Resolve(searchKeywords, DateTime.Now);
        }

        private string ResolveRoute(SearchEngine searchEngineType, string searchKeywords, int sequence)
        {
            IPageFetcherRouteResolver resolver = _routeResolvers.Where(c => c.SearchEngineType == searchEngineType).FirstOrDefault();
            if (resolver == null)
                throw new Exception("Route resolver not available for search engine: " + searchEngineType.ToString());

            return resolver.Resolve(sequence, searchKeywords);
        }

        private async Task<StorageClientUploadResponse> StoreHtmlToStorage(string fileName, string storeContent)
        {
            StorageClientUploadRequest request = new StorageClientUploadRequest
            {
                FileName = fileName,
                StoreContent = Encoding.UTF8.GetBytes(storeContent)
            };
            return await _storageClient.Upload(request);
        }

        private async Task<string> FetchHtml(string targetUrl)
        {
            string htmlContent = null;
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                HttpRequestMessage fetchRequest = new HttpRequestMessage(HttpMethod.Get, targetUrl);

                HttpResponseMessage response = await client.SendAsync(fetchRequest);
                htmlContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(htmlContent))
                    throw new Exception($"FetchHtml failed: Content returned from {targetUrl} (status: {response.StatusCode}) is empty");
            }
            catch (Exception ex)
            {
                //TODO:add logging
                throw new Exception($"FetchHtml failed: Fetch from {targetUrl} failed");
            }

            return htmlContent;
        }

        private void ValidateRequest(PageFetcherRequest request)
        {
            if (request.SearchKeywords.Contains("/"))
                throw new ArgumentException("SearchKeywords can not contain the character '/'");
        }

    }
}

