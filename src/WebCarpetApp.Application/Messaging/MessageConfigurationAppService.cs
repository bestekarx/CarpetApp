using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Messaging.Dtos;

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
    private readonly IRepository<MessageTask, Guid> _messageTaskRepository;
    private readonly IRepository<MessageTemplate, Guid> _messageTemplateRepository;

    public MessageConfigurationAppService(
        IRepository<MessageConfiguration, Guid> repository,
        IRepository<MessageTask, Guid> messageTaskRepository,
        IRepository<MessageTemplate, Guid> messageTemplateRepository)
        : base(repository)
    {
        _messageTaskRepository = messageTaskRepository;
        _messageTemplateRepository = messageTemplateRepository;
    }

    protected override async Task<IQueryable<MessageConfiguration>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        return await base.CreateFilteredQueryAsync(input);
    }

    protected override async Task<MessageConfiguration> MapToEntityAsync(CreateUpdateMessageConfigurationDto createInput)
    {
        var configuration = new MessageConfiguration(
            GuidGenerator.Create(),
            createInput.CompanyId,
            createInput.MessageUserId,
            createInput.Name,
            createInput.Description
        );

        return configuration;
    }

    public override async Task<MessageConfigurationDto> CreateAsync(CreateUpdateMessageConfigurationDto input)
    {
        var entity = await MapToEntityAsync(input);
        await Repository.InsertAsync(entity, autoSave: true);

        // Configuration kaydedildikten sonra MessageTask'ları ekle
        if (input.MessageTasks != null && input.MessageTasks.Any())
        {
            foreach (var taskDto in input.MessageTasks)
            {
                var messageTask = new MessageTask(
                    GuidGenerator.Create(),
                    entity.Id,
                    taskDto.TaskType,
                    taskDto.Behavior,
                    taskDto.CustomMessage
                );
                messageTask.SetActive(taskDto.Active);
                await _messageTaskRepository.InsertAsync(messageTask);
            }
        }

        // Configuration kaydedildikten sonra MessageTemplate'leri ekle
        if (input.MessageTemplates != null && input.MessageTemplates.Any())
        {
            foreach (var templateDto in input.MessageTemplates)
            {
                var messageTemplate = new MessageTemplate(
                    GuidGenerator.Create(),
                    entity.Id,
                    templateDto.TaskType,
                    templateDto.Name,
                    templateDto.Template,
                    templateDto.PlaceholderMappings,
                    templateDto.CultureCode
                );
                messageTemplate.SetActive(templateDto.Active);
                await _messageTemplateRepository.InsertAsync(messageTemplate);
            }
        }

        await CurrentUnitOfWork.SaveChangesAsync();
        return await MapToGetOutputDtoAsync(entity);
    }

    protected override async Task MapToEntityAsync(CreateUpdateMessageConfigurationDto updateInput, MessageConfiguration entity)
    {
        entity.UpdateMessageUser(updateInput.MessageUserId);
        entity.SetActive(updateInput.Active);

        // Mevcut task'ları güncelle veya yenilerini ekle
        if (updateInput.MessageTasks != null)
        {
            var existingTasks = await _messageTaskRepository.GetListAsync(x => x.MessageConfigurationId == entity.Id);
            foreach (var existingTask in existingTasks)
            {
                await _messageTaskRepository.DeleteAsync(existingTask);
            }
            foreach (var taskDto in updateInput.MessageTasks)
            {
                var messageTask = new MessageTask(
                    GuidGenerator.Create(),
                    entity.Id,
                    taskDto.TaskType,
                    taskDto.Behavior,
                    taskDto.CustomMessage
                );
                messageTask.SetActive(taskDto.Active);
                await _messageTaskRepository.InsertAsync(messageTask);
            }
        }

        // Mevcut template'leri güncelle veya yenilerini ekle
        if (updateInput.MessageTemplates != null)
        {
            var existingTemplates = await _messageTemplateRepository.GetListAsync(x => x.MessageConfigurationId == entity.Id);
            foreach (var existingTemplate in existingTemplates)
            {
                await _messageTemplateRepository.DeleteAsync(existingTemplate);
            }
            foreach (var templateDto in updateInput.MessageTemplates)
            {
                var messageTemplate = new MessageTemplate(
                    GuidGenerator.Create(),
                    entity.Id,
                    templateDto.TaskType,
                    templateDto.Name,
                    templateDto.Template,
                    templateDto.PlaceholderMappings,
                    templateDto.CultureCode
                );
                messageTemplate.SetActive(templateDto.Active);
                await _messageTemplateRepository.InsertAsync(messageTemplate);
            }
        }
    }

    public async Task<List<MessageTaskDto>> GetMessageTasksByConfigurationIdAsync(Guid configurationId)
    {
        var tasks = await _messageTaskRepository.GetListAsync(x => x.MessageConfigurationId == configurationId);
        return ObjectMapper.Map<List<MessageTask>, List<MessageTaskDto>>(tasks);
    }
} 