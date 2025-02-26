using System;
using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messaging;

public class MessageTemplate : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public MessageTaskType TaskType { get; private set; }
    public string Name { get; private set; }
    public string Template { get; private set; }
    public Dictionary<string, string> PlaceholderMappings { get; private set; }
    public bool IsActive { get; private set; }
    public string CultureCode { get; private set; }

    protected MessageTemplate()
    {
        PlaceholderMappings = new Dictionary<string, string>();
    }

    public MessageTemplate(
        Guid id,
        MessageTaskType taskType,
        string name,
        string template,
        Dictionary<string, string> placeholderMappings,
        string cultureCode = "tr-TR") : base(id)
    {
        TaskType = taskType;
        Name = name;
        Template = template;
        PlaceholderMappings = placeholderMappings;
        CultureCode = cultureCode;
        IsActive = true;
    }

    public void UpdateTemplate(string template)
    {
        Template = template;
    }

    public void UpdatePlaceholderMappings(Dictionary<string, string> mappings)
    {
        PlaceholderMappings = mappings;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public string FormatMessage(Dictionary<string, object> values)
    {
        var formattedMessage = Template;
        foreach (var mapping in PlaceholderMappings)
        {
            if (values.ContainsKey(mapping.Value))
            {
                formattedMessage = formattedMessage.Replace(mapping.Key, values[mapping.Value]?.ToString());
            }
        }
        return formattedMessage;
    }
} 