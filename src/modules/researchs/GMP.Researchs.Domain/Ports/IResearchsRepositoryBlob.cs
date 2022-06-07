using GMP.Researchs.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Ports
{
    public interface IResearchsRepositoryBlob
    {
        public Task<List<string>> GetNamesBlobs(string nameBlobContainer);

        public Task UploadFile(string nameBlobContainer, string nameFile, IFormFile dataFile, CancellationToken cancellationToken);
    }
}
