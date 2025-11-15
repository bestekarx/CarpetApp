namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new company
/// </summary>
public class CreateCompanyRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FirmColor { get; set; }
    public string MoneyUnit { get; set; }
    public bool HmdProcess { get; set; }
}

/// <summary>
/// Update existing company
/// </summary>
public class UpdateCompanyRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FirmColor { get; set; }
    public string MoneyUnit { get; set; }
    public bool HmdProcess { get; set; }
}
