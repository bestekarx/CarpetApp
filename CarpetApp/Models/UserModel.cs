namespace CarpetApp.Models;

public record UserModel : Entry
{
    public int AuthId { get; set; }
    public int VehicleId { get; set; }
    public int PrintTagId { get; set; }
    public int PrintNormalId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public bool Root { get; set; }
    public string OnesignalId { get; set; }
    public bool IsNotification { get; set; }
}