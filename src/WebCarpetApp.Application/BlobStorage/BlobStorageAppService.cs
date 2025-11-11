using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Domain.Repositories;

namespace WebCarpetApp.BlobStorage;

public class BlobStorageAppService : ApplicationService, IBlobStorageAppService
{
    private readonly IBlobContainerFactory _blobContainerFactory;
    private readonly IRepository<DatabaseBlob, Guid> _blobRepository;
    private readonly IRepository<DatabaseBlobContainer, Guid> _containerRepository;

    public BlobStorageAppService(
        IBlobContainerFactory blobContainerFactory,
        IRepository<DatabaseBlob, Guid> blobRepository,
        IRepository<DatabaseBlobContainer, Guid> containerRepository)
    {
        _blobContainerFactory = blobContainerFactory;
        _blobRepository = blobRepository;
        _containerRepository = containerRepository;
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

    public async Task<BlobResponseDto> SaveBlobAsync(string containerName, string blobName, BlobInfoDto input)
    {
        Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
        Check.NotNull(input, nameof(input));
        Check.NotNull(input.Content, nameof(input.Content));

        var containerEntity = await _containerRepository.FirstOrDefaultAsync(c => c.Name == containerName);
        if (containerEntity == null)
        {
            var container = _blobContainerFactory.Create(containerName);
            await container.SaveAsync(blobName, input.Content, overrideExisting: true);
            
            containerEntity = await _containerRepository.FirstOrDefaultAsync(c => c.Name == containerName);
            if (containerEntity == null)
            {
                throw new UserFriendlyException($"Container '{containerName}' could not be created!");
            }
        }
        else
        {
            var container = _blobContainerFactory.Create(containerName);
            await container.SaveAsync(blobName, input.Content, overrideExisting: true);
        }

        var blob = await _blobRepository.FirstOrDefaultAsync(
            b => b.ContainerId == containerEntity.Id && b.Name == blobName
        );

        if (blob == null)
        {
            throw new UserFriendlyException($"Blob '{blobName}' could not be found after saving!");
        }

        return new BlobResponseDto
        {
            BlobId = blob.Id,
            ContainerName = containerName,
            BlobName = blobName
        };
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

        var container = _blobContainerFactory.Create(containerName);
        await container.DeleteAsync(blobName);
    }
} 