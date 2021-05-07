using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Common.Models.StorageClient
{
    public class StorageClientDownloadRequest
    {
        /// <summary>
        /// NB: When downloading from Amazon S3, set this to the Key instead
        /// </summary>
        public string DownloadPath { get; set; } 
    }

    public class StorageClientDownloadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }
    }
}
