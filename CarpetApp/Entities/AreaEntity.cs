using SQLite;

namespace CarpetApp.Entities;


[Table("area")]
public class AreaEntity : CarpetApp.Entities.Base.Entry
{
    [Column("name")]  public string Name { get; set; }
}