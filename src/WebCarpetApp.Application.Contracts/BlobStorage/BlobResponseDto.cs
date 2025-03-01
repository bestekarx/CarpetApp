using System;

namespace WebCarpetApp.BlobStorage;

public class BlobResponseDto
{
    public Guid BlobId { get; set; }
    
    public string ContainerName { get; set; }
    
    public string BlobName { get; set; }
} 