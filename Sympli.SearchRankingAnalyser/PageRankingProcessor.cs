using Sympli.Common;
using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using Sympli.Common.Models.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Sympli.SearchRankingAnalyser.Helpers;

public class PageRankingProcessor
{
    private readonly IStorageClient _storageClient;
    private readonly IEnumerable<IHtmlRankingAnalyser> _htmlRankingAnalysers;

    public PageRankingProcessor(IStorageClient storageClient, IEnumerable<IHtmlRankingAnalyser> htmlRankingAnalysers)
    {
        _storageClient = storageClient;
        _htmlRankingAnalysers = htmlRankingAnalysers;
    }

    public async Task<PageRankingProcessorResponse> PageRankingProcessorHandler(PageRankingProcessorRequest request)
    {
        string targetPath = PathGenerator.Generate(request.SearchEngine, request.Keywords, request.TargetDate);

        StorageClientListFolderResponse files = await _storageClient.ListFolder(new StorageClientListFolderRequest { FolderPath = targetPath });

        List<Task<StorageClientDownloadResponse>> downloadTasks = files.FilePaths
                                                                        .Select(f => _storageClient.Download(new StorageClientDownloadRequest { DownloadPath = f }))
                                                                        .ToList();

        List<PageRankingProcessorWithSequenceResponse> processedResponses = new List<PageRankingProcessorWithSequenceResponse>();
        while (downloadTasks.Any())
        {
            Task<StorageClientDownloadResponse> finishedTask = await Task.WhenAny(downloadTasks);
            downloadTasks.Remove(finishedTask);

            StorageClientDownloadResponse downloadedData = await finishedTask;
            processedResponses.Add(await GenerateResponse(request.SearchEngine, downloadedData.FilePath, downloadedData.FileData));
        }

        //combine
        List<PageRank> pageRanks = new List<PageRank>();
        int rank = 1;
        foreach(var r in processedResponses.OrderBy(c => c.Sequence))
        {
            foreach (var p in r.Pages.OrderBy(c => c.Rank))
                pageRanks.Add(new PageRank { Rank = rank++, Url = p.Url });
        }

        return new PageRankingProcessorResponse
        {
            Date = request.TargetDate,
            Pages = pageRanks
        };
    }

    private async Task<PageRankingProcessorWithSequenceResponse> GenerateResponse(SearchEngine searchEngine, string filePath, byte[] fileData)
    {
        string result = System.Text.Encoding.UTF8.GetString(fileData);

        IHtmlRankingAnalyser analyser = _htmlRankingAnalysers.Where(c => c.AnalyserType == searchEngine).FirstOrDefault();
        if (analyser == null)
            throw new Exception("Html Ranking Analyser not found for " + searchEngine.ToString());

        return new PageRankingProcessorWithSequenceResponse
        {
            Date = GetDateFromFilePath(filePath),
            Sequence = GetSequenceFromFilePath(filePath),
            Pages = analyser.ProcessHtml(result)
        };
    }

    private int GetSequenceFromFilePath(string filePath)
    {
        try
        {
            Regex regex = new Regex("(?i)/(\\d+).htm", RegexOptions.Singleline);
            var matches = regex.Matches(filePath);

            return int.Parse(matches.First().Groups[1].Value);
        }
        catch (Exception ex)
        {
            string[] chopped = filePath.Split(new string[] { ".", "/" }, StringSplitOptions.RemoveEmptyEntries);

            return int.Parse(chopped[chopped.Length - 2]);
        }
    }

    private DateTime GetDateFromFilePath(string filePath)
    {
        try
        {
            Regex regex = new Regex("(?i)/(.+)/(\\d+).htm", RegexOptions.Singleline);
            var matches = regex.Matches(filePath);

            return DateTime.Parse(matches.First().Groups[1].Value);
        }
        catch (Exception ex)
        {
            string[] chopped = filePath.Split(new string[] { ".", "/" }, StringSplitOptions.RemoveEmptyEntries);

            return DateTime.Parse(chopped[chopped.Length - 3]);
        }
    }
}

