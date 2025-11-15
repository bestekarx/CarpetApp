using CarpetApp.Enums;

namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Product/service information
/// </summary>
public class ProductDto : AuditedEntityDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public EnProductType Type { get; set; }
}
