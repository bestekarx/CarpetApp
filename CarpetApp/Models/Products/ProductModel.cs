namespace CarpetApp.Models.Products;

public class ProductModel : AuditedEntity
{
    public decimal Price { get; set; }
    public required string Name { get; set; }
    public int Type { get; set; }
}