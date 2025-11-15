namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Company/firm information
/// </summary>
public class CompanyDto : AuditedEntityDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FirmColor { get; set; }
    public string MoneyUnit { get; set; }
    public bool HmdProcess { get; set; }
    public Guid? MessageSettingsId { get; set; }
}
