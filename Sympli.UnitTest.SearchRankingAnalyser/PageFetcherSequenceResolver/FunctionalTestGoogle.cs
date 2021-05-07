using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sympli.Common;
using Sympli.Common.Models.StorageClient;
using Sympli.SearchRankingAnalyser.Google;
using Sympli.SearchRankingAnalyser.Interface;
using Sympli.SearchRankingAnalyser.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sympli.UnitTest.SearchRankingAnalyser.PageFetcherSequenceResolver
{
    [TestClass]
    public class FunctionalTestGoogle
    {
        #region Google_ProcessFetcher_ResolveSequence_EmptyCase_DefaultMaxSequence
        [TestMethod]
        public async Task Google_ProcessFetcher_ResolveSequence_EmptyCase_DefaultMaxSequence()
        {
            IStorageClient storageClient = new S3ClientEmptyMock();
            IPageFetcherSequenceResolver resolver = new GooglePageFetcherSequenceResolver(storageClient);

            int expectedSequence = 1;
            int? actualSequence = await resolver.Resolve("any word", DateTime.Now);

            Assert.IsTrue(expectedSequence == actualSequence);
        }

        private class S3ClientEmptyMock : IStorageClient
        {
            public Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request)
            {
                throw new System.NotImplementedException();
            }

            public async Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request)
            {
                return new StorageClientListFolderResponse
                {
                    FilePaths = new List<string>()
                };
            }

            public Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request)
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region Google_ProcessFetcher_ResolveSequence_MaxCase_DefaultMaxSequence
        [TestMethod]
        public async Task Google_ProcessFetcher_ResolveSequence_MaxCase_DefaultMaxSequence()
        {
            IStorageClient storageClient = new S3ClientMaxMock();
            IPageFetcherSequenceResolver resolver = new GooglePageFetcherSequenceResolver(storageClient);

            int? expectedSequence = null;
            int? actualSequence = await resolver.Resolve("any word", DateTime.Now);

            Assert.IsTrue(expectedSequence == actualSequence);
        }

        private class S3ClientMaxMock : IStorageClient
        {
            public Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request)
            {
                throw new System.NotImplementedException();
            }

            public async Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request)
            {
                return new StorageClientListFolderResponse
                {
                    FilePaths = new List<string> {
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/1.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/2.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/3.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/4.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/5.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/6.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/7.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/8.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/9.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/10.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/11.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/12.html"
                    }
                };
            }

            public Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request)
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region Google_ProcessFetcher_ResolveSequence_EdgeCase_DefaultMaxSequence
        [TestMethod]
        public async Task Google_ProcessFetcher_ResolveSequence_EdgeCase_DefaultMaxSequence()
        {
            IStorageClient storageClient = new S3ClientEdgeMock();
            IPageFetcherSequenceResolver resolver = new GooglePageFetcherSequenceResolver(storageClient);

            int? expectedSequence = 12;
            int? actualSequence = await resolver.Resolve("any word", DateTime.Now);

            Assert.IsTrue(expectedSequence == actualSequence);
        }

        private class S3ClientEdgeMock : IStorageClient
        {
            public Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request)
            {
                throw new System.NotImplementedException();
            }

            public async Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request)
            {
                return new StorageClientListFolderResponse
                {
                    FilePaths = new List<string> {
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/1.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/2.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/3.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/4.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/5.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/6.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/7.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/8.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/9.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/10.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/11.html"
                    }
                };
            }

            public Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request)
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region Google_ProcessFetcher_ResolveSequence_AboveMaxCase_UpdatedMaxSequence
        [TestMethod]
        public async Task Google_ProcessFetcher_ResolveSequence_AboveMaxCase_UpdatedMaxSequence()
        {
            IStorageClient storageClient = new S3ClientMaxUpdatedSequenceMock();
            GooglePageFetcherSequenceResolver resolver = new GooglePageFetcherSequenceResolver(storageClient);

            resolver.MaxSequence = 10;

            int? expectedSequence = null;
            int? actualSequence = await resolver.Resolve("any word", DateTime.Now);

            Assert.IsTrue(expectedSequence == actualSequence);
        }

        private class S3ClientMaxUpdatedSequenceMock : IStorageClient
        {
            public Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request)
            {
                throw new System.NotImplementedException();
            }

            public async Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request)
            {
                return new StorageClientListFolderResponse
                {
                    FilePaths = new List<string> {
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/1.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/2.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/3.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/4.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/5.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/6.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/7.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/8.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/9.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/10.html"
                    }
                };
            }

            public Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request)
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region Google_ProcessFetcher_ResolveSequence_BelowMaxCase_UpdatedMaxSequence
        [TestMethod]
        public async Task Google_ProcessFetcher_ResolveSequence_BelowMaxCase_UpdatedMaxSequence()
        {
            IStorageClient storageClient = new S3ClientBelowMaxUpdatedSequenceMock();
            GooglePageFetcherSequenceResolver resolver = new GooglePageFetcherSequenceResolver(storageClient);

            resolver.MaxSequence = 10;

            int? expectedSequence = 5;
            int? actualSequence = await resolver.Resolve("any word", DateTime.Now);

            Assert.IsTrue(expectedSequence == actualSequence);
        }

        private class S3ClientBelowMaxUpdatedSequenceMock : IStorageClient
        {
            public Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request)
            {
                throw new System.NotImplementedException();
            }

            public async Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request)
            {
                return new StorageClientListFolderResponse
                {
                    FilePaths = new List<string> {
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/1.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/2.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/3.html",
                        "sympli/SearchRankings/Google/e-settlements/2021-04-24/4.html"
                    }
                };
            }

            public Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request)
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion
    }
}
