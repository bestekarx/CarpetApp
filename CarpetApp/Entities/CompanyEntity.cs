using SQLite;

namespace CarpetApp.Entities;

[Table("company")]
public class CompanyEntity : CarpetApp.Entities.Base.Entry
{
    [Column("message_settings_id")]  public int MessageSettingsId { get; set; }
    [Column("name")]  public string Name { get; set; }
    [Column("description")] public string Description { get; set; }
    [Column("color")] public string FirmColor { get; set; }
    [Column("money_unit")] public string MoneyUnit { get; set; }
    [Column("hmd_process")] public int HmdProcess { get; set; }
}