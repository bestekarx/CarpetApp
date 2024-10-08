using CarpetApp.Enums;
using SQLite;

namespace CarpetApp.Entities;

[Table("data_queue")]
public class DataQueueEntity : CarpetApp.Entities.Base.Entry
{
    [Column("package_no")]  public string PackageNo { get; set; }
    [Column("type")]  public EnSyncDataType Type { get; set; }
    [Column("json_data")]  public string JsonData { get; set; }
    [Column("date")]  public DateTime Date { get; set; }
}