namespace CarpetApp.Models;

public class CompanyModel : AuditedEntity
{
  public string Name { get; set; }
  public string Description { get; set; }

  public string Color { get; set; }
  /*public string MoneyUnit { get; set; }
  public int HmdProcess
  {
    get;
    set;
  } //How Many Day Process, işletme bir halıyı ya da X i teslim alınca kaç gün sonra teslim eder? tahmini.
  */
}