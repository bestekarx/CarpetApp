namespace CarpetApp.Models;

public record UserModel : Entry
{
    public decimal Id { get; set; }
    public int FirmId { get; set; }
    public int AuthId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public bool Active { get; set; }
    public bool Root { get; set; }
    public string notification_id { get; set; }
    public bool IsNotification { get; set; }
}