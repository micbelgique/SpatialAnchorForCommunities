using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services
{
    public class ImageService
    {
        private ImageServiceOption _options;

        public ImageService(IOptions<ImageServiceOption> options)
        {
            _options = options.Value;
        }

        public async Task<string> UploadImageAsync(Stream imageToUpload, Guid guid)
        {
            
            string containerName = DateTime.Now.ToString("yyyyMMMM").ToLower();
            string blobName = $"{guid.ToString()}.png";

            BlobContainerClient container = new BlobContainerClient(_options.AzureBlob,containerName);
            container.CreateIfNotExists(publicAccessType: PublicAccessType.Blob);

            BlobClient blob = container.GetBlobClient(blobName);
            var response = await blob.UploadAsync(imageToUpload, httpHeaders: new BlobHttpHeaders() { ContentType = "image/png" });

            return blob.Uri.ToString();
        }
    }
}
