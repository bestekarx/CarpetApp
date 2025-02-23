using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Messaging.Dtos;
using WebCarpetApp.Permissions;

namespace WebCarpetApp.Messaging;

public class MessageConfigurationAppService :
    CrudAppService<
        MessageConfiguration,
        MessageConfigurationDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageConfigurationDto>,
    IMessageConfigurationAppService
{
    public MessageConfigurationAppService(IRepository<MessageConfiguration, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebCarpetAppPermissions.MessageConfigurations.Default;
        GetListPolicyName = WebCarpetAppPermissions.MessageConfigurations.Default;
        CreatePolicyName = WebCarpetAppPermissions.MessageConfigurations.Create;
        UpdatePolicyName = WebCarpetAppPermissions.MessageConfigurations.Edit;
        DeletePolicyName = WebCarpetAppPermissions.MessageConfigurations.Delete;
    }

    protected override async Task<IQueryable<MessageConfiguration>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        return await base.CreateFilteredQueryAsync(input);
    }

    protected override async Task<MessageConfiguration> MapToEntityAsync(CreateUpdateMessageConfigurationDto createInput)
    {
        return new MessageConfiguration(
            GuidGenerator.Create(),
            createInput.CompanyId,
            createInput.MessageUserId,
            createInput.Name,
            createInput.Description
        );
    }

    protected override async Task MapToEntityAsync(CreateUpdateMessageConfigurationDto updateInput, MessageConfiguration entity)
    {
        entity.UpdateMessageUser(updateInput.MessageUserId);
        entity.SetActive(updateInput.IsActive);
    }

    public async Task<MessageConfigurationDto> AddMessageTaskAsync(Guid id, CreateUpdateMessageTaskDto input)
    {
        var configuration = await Repository.GetAsync(id);
        configuration.AddMessageTask(input.TaskType, input.Behavior, input.CustomMessage);
        await Repository.UpdateAsync(configuration);
        return ObjectMapper.Map<MessageConfiguration, MessageConfigurationDto>(configuration);
    }

    public async Task<MessageConfigurationDto> UpdateMessageTaskAsync(Guid id, Guid taskId, CreateUpdateMessageTaskDto input)
    {
        var configuration = await Repository.GetAsync(id);
        configuration.UpdateMessageTaskBehavior(taskId, input.Behavior);
        configuration.UpdateMessageTaskCustomMessage(taskId, input.CustomMessage);
        await Repository.UpdateAsync(configuration);
        return ObjectMapper.Map<MessageConfiguration, MessageConfigurationDto>(configuration);
    }

    public async Task<MessageConfigurationDto> RemoveMessageTaskAsync(Guid id, Guid taskId)
    {
        var configuration = await Repository.GetAsync(id);
        var task = configuration.MessageTasks.FirstOrDefault(x => x.Id == taskId);
        if (task != null)
        {
            task.SetActive(false);
            await Repository.UpdateAsync(configuration);
        }
        return ObjectMapper.Map<MessageConfiguration, MessageConfigurationDto>(configuration);
    }
} 