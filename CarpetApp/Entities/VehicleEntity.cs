using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("vehicle")]
public class VehicleEntity : Entry
{
  [Column("name")] public string Name { get; set; }
}