namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Service area/zone information
/// </summary>
public class AreaDto : AuditedEntityDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
