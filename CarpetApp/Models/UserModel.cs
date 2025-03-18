namespace CarpetApp.Models;

public record UserModel : AuditedEntity
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsExternal { get; set; }
    public bool HasPassword { get; set; }
    public string ConcurrencyStamp { get; set; }
    public Dictionary<string, object> ExtraProperties { get; set; } = new Dictionary<string, object>();
}


/*
public record UserModel : Entry
{
    public int AuthId { get; set; }
    public int VehicleId { get; set; }
    public int PrintTagId { get; set; }
    public int PrintNormalId { get; set; }
    public string UserName { get; set; }
    public string LoginCode { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public bool Root { get; set; }
    public string OnesignalId { get; set; }
    public bool IsNotification { get; set; }
}*/