using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Common.Models.StorageClient
{
    public class StorageClientListFolderRequest
    {
        public string FolderPath { get; set; }
    }
    public class StorageClientListFolderResponse
    {
        public List<string> FilePaths { get; set; }
    }
}
