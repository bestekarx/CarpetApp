namespace CarpetApp.Models;

public record CompanyModel : AuditedEntity
{
    public int MessageSettingsId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FirmColor { get; set; }
    public string MoneyUnit { get; set; }

    public int
        HmdProcess
    {
        get;
        set;
    } //How Many Day Process, işletme bir halıyı ya da X i teslim alınca kaç gün sonra teslim eder? tahmini.
}