namespace CarpetApp.Models;

public record AreaModel : AuditedEntity
{
    public string Name { get; set; }
}