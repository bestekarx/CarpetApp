using SQLite;

namespace CarpetApp.Entities;

[Table("vehicle")]
public class VehicleEntity : CarpetApp.Entities.Base.Entry
{
    [Column("name")]  public string Name { get; set; }
}