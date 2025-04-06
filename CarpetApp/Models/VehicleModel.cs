namespace CarpetApp.Models;

public class VehicleModel : AuditedEntity
{
    public string Name { get; set; }
    public string Plate { get; set; }
}