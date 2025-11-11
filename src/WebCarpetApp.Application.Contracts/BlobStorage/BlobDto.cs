using System;

namespace WebCarpetApp.BlobStorage;

public class BlobDto
{
    public required byte[] Content { get; set; }
    
    public required string Name { get; set; }
    
    public string? ContentType { get; set; }
} 