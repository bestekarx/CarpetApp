using CarpetApp.Enums;

namespace CarpetApp.Models.MessageTaskModels;
public class MessageTaskModel
{
    public Guid Id { get; set; }
    public MessageTaskType TaskType { get; set; }
    public string TaskTypeName { get; set; }
    public MessageBehavior Behaviour { get; set; }
    public string BehaviourName { get; set; }
    public string Name { get; set; }
    public string Template { get; set; }
    public string CustomMessage { get; set; } = string.Empty;
    public string MessageTemplateShort => !string.IsNullOrEmpty(Template) && Template.Length > 30 ? Template.Substring(0, 30) + "..." : Template;
} 