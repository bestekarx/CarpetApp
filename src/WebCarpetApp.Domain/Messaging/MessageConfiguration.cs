using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messaging;

public class MessageConfiguration : AggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid MessageUserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool Active { get; private set; }
    
    public ICollection<MessageTask> MessageTasks { get; set; }
    public ICollection<MessageTemplate> MessageTemplates { get; set; }

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
        Active = true;
    }

    public void SetActive(bool active)
    {
        Active = active;
    }

    public void UpdateMessageUser(Guid messageUserId)
    {
        MessageUserId = messageUserId;
    }
} 