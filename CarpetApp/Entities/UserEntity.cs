using SQLite;

namespace CarpetApp.Entities;

[Table("users")]
public class UserEntity : CarpetApp.Entities.Base.Entry
{
    [Column("auth_id")]  public int AuthId { get; set; }
    [Column("vehicle_id")]  public int VehicleId { get; set; }
    [Column("print_tag_id")]  public int PrintTagId { get; set; }
    [Column("print_normal_id")]  public int PrintNormalId { get; set; }
    [Column("username")] public string UserName { get; set; }
    [Column("password")] public string Password { get; set; }
    [Column("fullname")] public string FullName { get; set; }
    [Column("root")] public bool Root { get; set; }
    [Column("onesignal_id")] public string OnesignalId { get; set; }
    [Column("is_notification")] public bool IsNotification { get; set; }
}