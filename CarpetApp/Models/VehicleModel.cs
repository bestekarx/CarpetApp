namespace CarpetApp.Models;

public record VehicleModel : AuditedEntity
{
    public string Name { get; set; }
}