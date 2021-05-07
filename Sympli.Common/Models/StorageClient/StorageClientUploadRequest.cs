using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Common.Models.StorageClient
{
    public class StorageClientUploadRequest
    {
        public string FileName { get; set; }
        public byte[] StoreContent { get; set; }
    }
    public class StorageClientUploadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
