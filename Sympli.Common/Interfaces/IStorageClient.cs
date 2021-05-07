using Sympli.Common.Models.StorageClient;
using System;
using System.Threading.Tasks;

namespace Sympli.Common
{
    public interface IStorageClient
    {
        Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request);
        Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request);
        Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request);
    }
}
