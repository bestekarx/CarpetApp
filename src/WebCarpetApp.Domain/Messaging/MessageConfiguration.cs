using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messaging;

public class MessageConfiguration : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid MessageUserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<MessageTask> _messageTasks;
    public IReadOnlyCollection<MessageTask> MessageTasks => _messageTasks.AsReadOnly();

    protected MessageConfiguration()
    {
        _messageTasks = new List<MessageTask>();
    }

    public MessageConfiguration(
        Guid id,
        Guid companyId,
        Guid messageUserId,
        string name,
        string description = null) : base(id)
    {
        CompanyId = companyId;
        MessageUserId = messageUserId;
        Name = name;
        Description = description;
        IsActive = true;
        _messageTasks = new List<MessageTask>();
    }

    public void AddMessageTask(MessageTaskType taskType, MessageBehavior behavior, string customMessage = null)
    {
        _messageTasks.Add(new MessageTask(Guid.NewGuid(), Id, taskType, behavior, customMessage));
    }

    public void UpdateMessageTaskBehavior(Guid taskId, MessageBehavior newBehavior)
    {
        var task = _messageTasks.Find(x => x.Id == taskId);
        if (task != null)
        {
            task.UpdateBehavior(newBehavior);
        }
    }

    public void UpdateMessageTaskCustomMessage(Guid taskId, string newMessage)
    {
        var task = _messageTasks.Find(x => x.Id == taskId);
        if (task != null)
        {
            task.UpdateCustomMessage(newMessage);
        }
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void UpdateMessageUser(Guid messageUserId)
    {
        MessageUserId = messageUserId;
    }
} 