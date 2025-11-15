using CarpetApp.Enums;

namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new product/service
/// </summary>
public class CreateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public EnProductType Type { get; set; }
    public bool Active { get; set; } = true;
}

/// <summary>
/// Update existing product
/// </summary>
public class UpdateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public EnProductType Type { get; set; }
    public bool Active { get; set; }
}
