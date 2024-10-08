using SQLite;

namespace CarpetApp.Entities;

[Table("sms_users")]
public class SmsUsersEntity : CarpetApp.Entities.Base.Entry
{
    [Column("username")]  public string UserName { get; set; }
    [Column("password")]  public string Password { get; set; }
    [Column("title")]  public string Title { get; set; }
}