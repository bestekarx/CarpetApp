namespace CarpetApp.Models;

public record SmsUsersModel : AuditedEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
}