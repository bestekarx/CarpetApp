using CarpetApp.Enums;
using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("data_queue")]
public class DataQueueEntity : Entry
{
  [Column("package_no")] public string PackageNo { get; set; }
  [Column("type")] public EnSyncDataType Type { get; set; }
  [Column("json_data")] public string JsonData { get; set; }
  [Column("date")] public DateTime Date { get; set; }
}