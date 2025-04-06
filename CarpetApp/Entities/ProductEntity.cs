using CarpetApp.Enums;
using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("product")]
public class ProductEntity : Entry
{
  [Column("price")] public decimal Price { get; set; }
  [Column("name")] public string Name { get; set; }
  [Column("type")] public EnProductType Type { get; set; }
}