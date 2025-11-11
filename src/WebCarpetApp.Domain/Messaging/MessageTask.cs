using System;
using Volo.Abp.Domain.Entities;

namespace WebCarpetApp.Messaging;

public class MessageTask : Entity<Guid>
{
    public Guid MessageConfigurationId { get; private set; }
    public MessageTaskType TaskType { get; private set; }
    public MessageBehavior Behavior { get; private set; }
    public string CustomMessage { get; private set; }
    public bool Active { get; private set; }

    protected MessageTask() { }

    public MessageTask(
        Guid id,
        Guid messageConfigurationId,
        MessageTaskType taskType,
        MessageBehavior behavior,
        string customMessage = null) : base(id)
    {
        MessageConfigurationId = messageConfigurationId;
        TaskType = taskType;
        Behavior = behavior;
        CustomMessage = customMessage;
        Active = true;
    }

    public  void UpdateBehavior(MessageBehavior newBehavior)
    {
        Behavior = newBehavior;
    }

    public  void UpdateCustomMessage(string newMessage)
    {
        CustomMessage = newMessage;
    }

    public  void SetActive(bool active)
    {
        Active = active;
    }
} 