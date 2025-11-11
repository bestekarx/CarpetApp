using System;

namespace WebCarpetApp.BlobStorage;

public class BlobInfoDto
{
    public byte[] Content { get; set; }
    
    public string ContentType { get; set; }
} 