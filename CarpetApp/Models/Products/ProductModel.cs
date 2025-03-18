using CarpetApp.Enums;

namespace CarpetApp.Models.Products;

public record ProductModel : AuditedEntity
{
    public Guid UserId { get; set; }
    public decimal Price { get; set; }
    public required string Name { get; set; }
    public EnProductType Type { get; set; }
    public bool Active { get; set; }
}