namespace CarpetApp.Models;

public class SmsConfigurationModel : AuditedEntity
{
  public Guid CompanyId { get; set; }
  public Guid MessageUserId { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
}