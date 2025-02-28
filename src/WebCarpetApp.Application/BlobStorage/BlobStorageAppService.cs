using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace WebCarpetApp.BlobStorage;

public class BlobStorageAppService : ApplicationService, IBlobStorageAppService
{
    private readonly IBlobContainerFactory _blobContainerFactory;

    public BlobStorageAppService(IBlobContainerFactory blobContainerFactory)
    {
        _blobContainerFactory = blobContainerFactory;
    }

    public async Task<BlobDto> GetBlobAsync(string containerName, string blobName)
    {
        Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

        var container = _blobContainerFactory.Create(containerName);
        
        if (!await container.ExistsAsync(blobName))
        {
            throw new UserFriendlyException($"Could not find the requested blob '{blobName}' in the container '{containerName}'!");
        }

        var blob = await container.GetAllBytesAsync(blobName);
        
        // ContentType için basit bir çözüm (gerçek uygulamada metadata kullanabilirsiniz)
        string contentType = "application/octet-stream";
        if (blobName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || 
            blobName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
        {
            contentType = "image/jpeg";
        }
        else if (blobName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            contentType = "image/png";
        }
        else if (blobName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
        {
            contentType = "image/gif";
        }
        else if (blobName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            contentType = "application/pdf";
        }

        return new BlobDto
        {
            Content = blob,
            Name = blobName,
            ContentType = contentType
        };
    }

    public async Task SaveBlobAsync(string containerName, string blobName, BlobInfoDto input)
    {
        Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
        Check.NotNull(input, nameof(input));
        Check.NotNull(input.Content, nameof(input.Content));

        var container = _blobContainerFactory.Create(containerName);
        await container.SaveAsync(blobName, input.Content, overrideExisting: true);
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

        var container = _blobContainerFactory.Create(containerName);
        await container.DeleteAsync(blobName);
    }
} 