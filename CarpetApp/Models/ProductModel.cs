using CarpetApp.Enums;

namespace CarpetApp.Models;

public record ProductModel : Entry
{
    public decimal Price { get; set; }
    public string Name { get; set; }
    public EnProductType Type { get; set; } // 0 = Hizmet, 1 = sabit ürün, 2 = Fason
}