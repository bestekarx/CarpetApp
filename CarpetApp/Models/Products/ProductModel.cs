namespace CarpetApp.Models.Products;

public record ProductModel : AuditedEntity
{
    public decimal Price { get; set; }
    public required string Name { get; set; }
    public int Type { get; set; }
    public bool Active { get; set; }
}