using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Sympli.Common.Models.StorageClient;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Amazon;

namespace Sympli.Common.AWS
{
    public class S3Client : IStorageClient
    {
        string accesskey = "INSERT KEY HERE";
        string secretkey = "INSERT SECRET HERE";
        RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        string bucketName = "sympli";

        public async Task<StorageClientUploadResponse> Upload(StorageClientUploadRequest request)
        {
            AmazonS3Client s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            TransferUtility fileTransferUtility = new TransferUtility(s3Client);
            try
            {
                byte[] byteArray = request.StoreContent;
                using (MemoryStream dataStream = new MemoryStream(byteArray))
                {
                    TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        StorageClass = S3StorageClass.Standard,
                        PartSize = 6291456, // 6 MB
                        Key = request.FileName,
                        CannedACL = S3CannedACL.Private,
                        ContentType = "text/html",
                        InputStream = dataStream
                    };
                    await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                    fileTransferUtility.Dispose();
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return new StorageClientUploadResponse { Success = false, Message = "Check the provided AWS Credentials." };
                }
                else
                {
                    return new StorageClientUploadResponse { Success = false, Message = $"Error occurred: {amazonS3Exception.Message}" };
                }
            }

            return new StorageClientUploadResponse { Success = true };
        }

        public async Task<StorageClientDownloadResponse> Download(StorageClientDownloadRequest request)
        {
            AmazonS3Client s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            TransferUtility fileTransferUtility = new TransferUtility(s3Client);
            try
            {
                string tempFileLocation = Path.GetTempFileName();

                TransferUtilityDownloadRequest fileTransferUtilityRequest = new TransferUtilityDownloadRequest
                {
                    BucketName = bucketName,
                    FilePath = tempFileLocation,
                    Key = request.DownloadPath
                };
                await fileTransferUtility.DownloadAsync(fileTransferUtilityRequest);
                fileTransferUtility.Dispose();

                byte[] fileData = await File.ReadAllBytesAsync(tempFileLocation);

                //try clean up
                try
                {
                    File.Delete(tempFileLocation);
                }
                catch { 
                    //not the worse if it fails...
                }
                return new StorageClientDownloadResponse { FilePath = request.DownloadPath, FileData = fileData };
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return new StorageClientDownloadResponse { Success = false, Message = "Check the provided AWS Credentials." };
                }
                else
                {
                    return new StorageClientDownloadResponse { Success = false, Message = $"Error occurred: {amazonS3Exception.Message}" };
                }
            }
        }

        public async Task<StorageClientListFolderResponse> ListFolder(StorageClientListFolderRequest request)
        {
            AmazonS3Client s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            ListObjectsV2Request listObjReq = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = request.FolderPath
            };
            ListObjectsV2Response listObjResponse = await s3Client.ListObjectsV2Async(listObjReq);

            return new StorageClientListFolderResponse { FilePaths = listObjResponse.S3Objects.Select(c => c.Key).ToList() };
        }


    }
}
