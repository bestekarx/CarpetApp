using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("sms_users")]
public class SmsUsersEntity : Entry
{
    [Column("username")] public string UserName { get; set; }
    [Column("password")] public string Password { get; set; }
    [Column("title")] public string Title { get; set; }
}