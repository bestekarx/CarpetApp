using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Products.Dtos;

public class ProductDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public bool Active { get; set; }
} 