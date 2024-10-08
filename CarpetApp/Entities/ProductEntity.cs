using CarpetApp.Enums;
using SQLite;

namespace CarpetApp.Entities;

[Table("product")]
public class ProductEntity : CarpetApp.Entities.Base.Entry
{
    [Column("price")]  public decimal Price { get; set; }
    [Column("name")]  public string Name { get; set; }
    [Column("type")]  public EnProductType Type { get; set; }
}