using CarpetApp.Enums;

namespace CarpetApp.Models;

public class MessageTaskModel
{
    public MessageTaskType TaskType { get; set; }
    public string TaskTypeName { get; set; }
    public MessageBehavior Behaviour { get; set; }
    public string BehaviourName { get; set; }
    public string Name { get; set; }
    public string Template { get; set; }
    public string MessageTemplateShort => !string.IsNullOrEmpty(Template) && Template.Length > 30 ? Template.Substring(0, 30) + "..." : Template;
} 