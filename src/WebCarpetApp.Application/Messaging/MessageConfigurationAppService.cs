using System;
using System.Collections.Generic;
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
    private readonly IRepository<MessageTask, Guid> _messageTaskRepository;

    public MessageConfigurationAppService(
        IRepository<MessageConfiguration, Guid> repository,
        IRepository<MessageTask, Guid> messageTaskRepository)
        : base(repository)
    {
        _messageTaskRepository = messageTaskRepository;
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

        if (createInput.MessageTasks != null && createInput.MessageTasks.Any())
        {
            foreach (var taskDto in createInput.MessageTasks)
            {
                var messageTask = new MessageTask(
                    GuidGenerator.Create(),
                    configuration.Id,
                    taskDto.TaskType,
                    taskDto.Behavior,
                    taskDto.CustomMessage
                );
                messageTask.SetActive(taskDto.Active);
                await _messageTaskRepository.InsertAsync(messageTask);
            }
        }

        return configuration;
    }

    protected override async Task MapToEntityAsync(CreateUpdateMessageConfigurationDto updateInput, MessageConfiguration entity)
    {
        entity.UpdateMessageUser(updateInput.MessageUserId);
        entity.SetActive(updateInput.Active);

        // Mevcut task'ları güncelle veya yenilerini ekle
        if (updateInput.MessageTasks != null)
        {
            var existingTasks = await _messageTaskRepository.GetListAsync(x => x.MessageConfigurationId == entity.Id);
            
            // Mevcut task'ları sil
            foreach (var existingTask in existingTasks)
            {
                await _messageTaskRepository.DeleteAsync(existingTask);
            }

            // Yeni task'ları ekle
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
    }

    public async Task<List<MessageTaskDto>> GetMessageTasksByConfigurationIdAsync(Guid configurationId)
    {
        var tasks = await _messageTaskRepository.GetListAsync(x => x.MessageConfigurationId == configurationId);
        return ObjectMapper.Map<List<MessageTask>, List<MessageTaskDto>>(tasks);
    }
} 