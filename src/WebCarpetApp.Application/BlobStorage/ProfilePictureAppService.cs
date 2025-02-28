using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using WebCarpetApp.BlobStoring;

namespace WebCarpetApp.BlobStorage;

[Authorize]
public class ProfilePictureAppService : ApplicationService
{
    private readonly IBlobContainer<ProfilePictureContainer> _profilePictureContainer;

    public ProfilePictureAppService(IBlobContainer<ProfilePictureContainer> profilePictureContainer)
    {
        _profilePictureContainer = profilePictureContainer;
    }

    public async Task<ProfilePictureDto> GetProfilePictureAsync(Guid userId)
    {
        var blobName = GetBlobName(userId);
        
        if (!await _profilePictureContainer.ExistsAsync(blobName))
        {
            return null;
        }

        var bytes = await _profilePictureContainer.GetAllBytesAsync(blobName);
        
        return new ProfilePictureDto
        {
            Name = blobName,
            Content = bytes,
            ContentType = "image/jpeg" // Profil resimlerini JPEG formatında kaydediyoruz varsayımıyla
        };
    }

    public async Task SaveProfilePictureAsync(Guid userId, SaveProfilePictureDto input)
    {
        Check.NotNull(input, nameof(input));
        Check.NotNull(input.Content, nameof(input.Content));

        var blobName = GetBlobName(userId);
        
        await _profilePictureContainer.SaveAsync(blobName, input.Content, overrideExisting: true);
    }

    public async Task DeleteProfilePictureAsync(Guid userId)
    {
        var blobName = GetBlobName(userId);
        
        if (await _profilePictureContainer.ExistsAsync(blobName))
        {
            await _profilePictureContainer.DeleteAsync(blobName);
        }
    }

    private string GetBlobName(Guid userId)
    {
        return $"{userId}.jpg";
    }
} 