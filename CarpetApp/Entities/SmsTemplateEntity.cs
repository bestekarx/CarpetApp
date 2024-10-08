using SQLite;

namespace CarpetApp.Entities;

[Table("sms_templates")]
public class SmsTemplateEntity : CarpetApp.Entities.Base.Entry
{
    [Column("title")]  public string Title { get; set; }
    [Column("content")]  public string Content { get; set; }
}