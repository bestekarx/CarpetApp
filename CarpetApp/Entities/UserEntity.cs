using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("users")]
public class UserEntity : Entry
{
    [Column("id")]
    public int Id { get; set; }

    [Column("firm_id")]
    public int FirmId { get; set; }

    [Column("auth_id")]
    public int AuthId { get; set; }
  
    [Column("username")]
    public string Username { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("fullname")]
    public string Fullname { get; set; }
    
    [Column("active")]
    public int Active { get; set; }

    [Column("root")] 
    public int Root { get; set; }

    [Column("notification_id")]
    public string notification_id { get; set; }
    
    [Column("is_notification")]
    public int IsNotification { get; set; }
  
}