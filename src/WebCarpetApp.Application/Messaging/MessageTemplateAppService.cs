using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Messaging.Dtos;
using WebCarpetApp.Permissions;

namespace WebCarpetApp.Messaging;

public class MessageTemplateAppService :
    CrudAppService<
        MessageTemplate,
        MessageTemplateDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageTemplateDto>,
    IMessageTemplateAppService
{
    public MessageTemplateAppService(IRepository<MessageTemplate, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebCarpetAppPermissions.MessageTemplates.Default;
        GetListPolicyName = WebCarpetAppPermissions.MessageTemplates.Default;
        CreatePolicyName = WebCarpetAppPermissions.MessageTemplates.Create;
        UpdatePolicyName = WebCarpetAppPermissions.MessageTemplates.Edit;
        DeletePolicyName = WebCarpetAppPermissions.MessageTemplates.Delete;
    }

    protected override async Task<MessageTemplate> MapToEntityAsync(CreateUpdateMessageTemplateDto createInput)
    {
        return new MessageTemplate(
            GuidGenerator.Create(),
            createInput.TaskType,
            createInput.Name,
            createInput.Template,
            createInput.PlaceholderMappings,
            createInput.CultureCode
        );
    }

    protected override async Task MapToEntityAsync(CreateUpdateMessageTemplateDto updateInput, MessageTemplate entity)
    {
        entity.UpdateTemplate(updateInput.Template);
        entity.UpdatePlaceholderMappings(updateInput.PlaceholderMappings);
        entity.SetActive(updateInput.IsActive);
    }

    public async Task<string> FormatMessageAsync(Guid id, Dictionary<string, object> values)
    {
        var template = await Repository.GetAsync(id);
        return template.FormatMessage(values);
    }

    public async Task<MessageTemplateDto> GetByTaskTypeAsync(MessageTaskType taskType, string cultureCode = "tr-TR")
    {
        var template = await Repository.FirstOrDefaultAsync(x => 
            x.TaskType == taskType && 
            x.CultureCode == cultureCode &&
            x.IsActive);

        if (template == null)
        {
            // Eğer belirtilen dilde şablon bulunamazsa, varsayılan dildeki şablonu dene
            template = await Repository.FirstOrDefaultAsync(x => 
                x.TaskType == taskType && 
                x.CultureCode == "tr-TR" &&
                x.IsActive);
        }

        return ObjectMapper.Map<MessageTemplate, MessageTemplateDto>(template);
    }
} 