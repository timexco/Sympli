using Sympli.Common;
using Sympli.Common.Models.StorageClient;
using Sympli.SearchRankingAnalyser.Helpers;
using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sympli.SearchRankingAnalyser.Google
{
    public class GooglePageFetcherSequenceResolver : IPageFetcherSequenceResolver
    {
        public int MaxSequence { get; set; } = 12; //default to 12, each page returns 10 results, 12 = 120 results. Target is 100, extra 20 for "backup"
        public SearchEngine SearchEngineType => SearchEngine.Google;

        private readonly IStorageClient _storageClient;

        public GooglePageFetcherSequenceResolver(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<int?> Resolve(string keywords, DateTime targetDate)
        {
            //fetch files from S3 and check for the next sequence to fetch
            string targetPath = PathGenerator.Generate(SearchEngine.Google, keywords, targetDate);

            StorageClientListFolderResponse files = await _storageClient.ListFolder(new StorageClientListFolderRequest { FolderPath = targetPath });

            //expected format:  sympli/SearchRankings/Google/e-settlements/2021-04-24/1.html
            //                  sympli/SearchRankings/Google/e-settlements/2021-04-24/2.html
            //                  ...
            if (files == null || files.FilePaths == null || files.FilePaths.Count == 0)
                return 1;

            //duplicate! TODO: move and refractor
            Regex regex = new Regex("(?i)/(\\d+).htm", RegexOptions.Singleline);

            List<int> sequencesFromFilePaths = files.FilePaths.Select(filePath =>
                                                                        {
                                                                            var matches = regex.Matches(filePath);
                                                                            if (matches.Any())
                                                                            {
                                                                                bool numberFound = int.TryParse(matches.First().Groups[1].Value, out int sequence);
                                                                                return numberFound ? sequence : 0;
                                                                            }
                                                                            return 0;
                                                                        }).ToList();

            if (sequencesFromFilePaths.Max() + 1 <= MaxSequence)
                return sequencesFromFilePaths.Max() + 1;
            else
                return null; //max sequence exceeded, return NULL to terminate process.
        }
    }
}
