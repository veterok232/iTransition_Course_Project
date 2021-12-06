using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Course_project.CloudStorage
{
    /// <summary>
    /// Interface for GoogleCloudStorage
    /// </summary>
    public interface ICloudStorage
    {
        /// <summary>
        /// Upload file to Google Cloud storage
        /// </summary>
        /// <param name="imageFile">File to upload</param>
        /// <param name="fileNameForStorage">File name</param>
        /// <returns>Task<string></returns>
        Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage);

        /// <summary>
        /// Delete file from Google Cloud storage
        /// </summary>
        /// <param name="fileNameForStorage">File name</param>
        /// <returns>Task</returns>
        Task DeleteFileAsync(string fileNameForStorage);
    }
}
