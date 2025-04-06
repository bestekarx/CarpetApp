using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("area")]
public class AreaEntity : Entry
{
  [Column("name")] public string Name { get; set; }
}