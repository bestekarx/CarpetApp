namespace CarpetApp.Models.MessageTaskModels;

public class SmsConfigurationModel : AuditedEntity
{
  public Guid CompanyId { get; set; }
  public Guid MessageUserId { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public List<MessageTaskModel> MessageTasks { get; set; } = [];
  public List<MessageTemplate> MessageTemplates { get; set; } = [];
}