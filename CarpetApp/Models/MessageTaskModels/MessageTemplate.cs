using CarpetApp.Enums;

namespace CarpetApp.Models.MessageTaskModels;

public class MessageTemplate
{
    public Guid? Id { get;  set; }
    public Guid? TenantId { get;  set; }
    public MessageTaskType TaskType { get;  set; }
    public string Name { get;  set; }
    public string Template { get;  set; }
    public Dictionary<string, string> PlaceholderMappings { get;  set; }
    public bool Active { get;  set; }
    public string CultureCode { get;  set; }
}