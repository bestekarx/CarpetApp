using SQLite;
using Entry = CarpetApp.Entities.Base.Entry;

namespace CarpetApp.Entities;

[Table("sms_templates")]
public class SmsTemplateEntity : Entry
{
    [Column("title")] public string Title { get; set; }
    [Column("content")] public string Content { get; set; }
}