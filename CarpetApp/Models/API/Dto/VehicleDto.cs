namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Delivery vehicle information
/// </summary>
public class VehicleDto : AuditedEntityDto
{
    public string Name { get; set; }
    public string Plate { get; set; }
    public string Description { get; set; }
}
