namespace CarpetApp.Models;

public record SmsTemplateModel : AuditedEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
}