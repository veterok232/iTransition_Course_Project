using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Course_project.CloudStorage
{
    /// <summary>
    /// GoogleCloudStorage class
    /// </summary>
    public class GoogleCloudStorage : ICloudStorage
    {
        /// <summary>
        /// Google credential
        /// </summary>
        private readonly GoogleCredential googleCredential;

        /// <summary>
        /// Storage client
        /// </summary>
        private readonly StorageClient storageClient;

        /// <summary>
        /// Bucket name
        /// </summary>
        private readonly string bucketName;

        /// <summary>
        /// Constructor of GoogleCloudStorage class
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public GoogleCloudStorage(IConfiguration configuration)
        {
            googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
            storageClient = StorageClient.Create(googleCredential);
            bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket");
        }

        /// <summary>
        /// Delete file from Google Cloud storage
        /// </summary>
        /// <param name="fileNameForStorage">File name</param>
        /// <returns>Task</returns>
        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await storageClient.DeleteObjectAsync(bucketName, fileNameForStorage);
        }

        /// <summary>
        /// Upload file to Google Cloud storage
        /// </summary>
        /// <param name="imageFile">File to upload</param>
        /// <param name="fileNameForStorage">File name</param>
        /// <returns>Task<string></returns>
        public async Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                var dataObject = await storageClient.UploadObjectAsync(bucketName, fileNameForStorage, null, memoryStream);
                return dataObject.MediaLink;
            }
        }
    }
}
