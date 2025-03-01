using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebCarpetApp.BlobStorage;

public interface IBlobStorageAppService : IApplicationService
{
    Task<BlobDto> GetBlobAsync(string containerName, string blobName);
    
    Task<BlobResponseDto> SaveBlobAsync(string containerName, string blobName, BlobInfoDto input);
    
    Task DeleteBlobAsync(string containerName, string blobName);
} 