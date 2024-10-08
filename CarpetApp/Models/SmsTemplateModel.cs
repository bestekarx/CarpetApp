namespace CarpetApp.Models;

public record SmsTemplateModel : Entry
{
    public string Title { get; set; }
    public string Content { get; set; }
}