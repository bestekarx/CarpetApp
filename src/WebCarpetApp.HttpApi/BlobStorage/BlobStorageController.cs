using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace WebCarpetApp.BlobStorage;

[Route("api/blob-storage")]
public class BlobStorageController : AbpController
{
    private readonly IBlobStorageAppService _blobStorageAppService;

    public BlobStorageController(IBlobStorageAppService blobStorageAppService)
    {
        _blobStorageAppService = blobStorageAppService;
    }

    [HttpGet]
    [Route("{containerName}/{blobName}")]
    public async Task<IActionResult> GetBlobAsync(string containerName, string blobName)
    {
        var blob = await _blobStorageAppService.GetBlobAsync(containerName, blobName);
        return File(blob.Content, blob.ContentType, blob.Name);
    }

    [HttpPost]
    [Route("{containerName}/{blobName}")]
    public async Task<IActionResult> SaveBlobAsync(string containerName, string blobName, IFormFile file)
    {
        Check.NotNull(file, nameof(file));

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            
            var blobInfoDto = new BlobInfoDto
            {
                Content = memoryStream.ToArray(),
                ContentType = file.ContentType
            };
            
            await _blobStorageAppService.SaveBlobAsync(containerName, blobName, blobInfoDto);
            
            return Ok();
        }
    }

    [HttpDelete]
    [Route("{containerName}/{blobName}")]
    public async Task<IActionResult> DeleteBlobAsync(string containerName, string blobName)
    {
        await _blobStorageAppService.DeleteBlobAsync(containerName, blobName);
        return NoContent();
    }
} 