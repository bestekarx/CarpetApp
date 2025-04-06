namespace CarpetApp.Models;

public class SmsTemplateModel : AuditedEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
}