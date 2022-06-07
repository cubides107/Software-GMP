using Azure.Storage.Blobs;
using GMP.Infrastructure.Exceptions;
using GMP.Researchs.Domain.Entities;
using GMP.Researchs.Domain.Ports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.Infrastructure.Repositories
{
    public class ResearchsRepositoryBlob : IResearchsRepositoryBlob
    {
        private readonly BlobServiceClient blobServiceClient;

        public ResearchsRepositoryBlob(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<List<string>> GetNamesBlobs(string nameBlobContainer)
        {
            var container = this.blobServiceClient.GetBlobContainerClient(nameBlobContainer);
            var items = new List<string>();

            await foreach (var blob in container.GetBlobsAsync())
            {
                items.Add(blob.Name);
            }

            return items;
        }

        public async Task UploadFile(string nameBlobContainer, string nameFile, IFormFile dataFile, CancellationToken cancellationToken)
        {
            var container = this.blobServiceClient.GetBlobContainerClient(nameBlobContainer);

            try
            {
                await container.UploadBlobAsync(nameFile, dataFile.OpenReadStream(), cancellationToken);
            }
            catch (BlobDbException e)
            {
                throw new SqlDbException($"{BlobDbException.MESSAGE_NOT_SAVE} {e.Message}");
            }
        }

    }
}
