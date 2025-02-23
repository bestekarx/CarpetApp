using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace WebCarpetApp.Messaging;

public class MessageManager : DomainService
{
    private readonly IRepository<MessageConfiguration, Guid> _configurationRepository;
    private readonly IRepository<MessageTask, Guid> _taskRepository;

    public MessageManager(
        IRepository<MessageConfiguration, Guid> configurationRepository,
        IRepository<MessageTask, Guid> taskRepository)
    {
        _configurationRepository = configurationRepository;
        _taskRepository = taskRepository;
    }

    public async Task<MessageBehavior?> GetMessageBehaviorAsync(Guid companyId, MessageTaskType taskType)
    {
        var configuration = await _configurationRepository.FirstOrDefaultAsync(x => 
            x.CompanyId == companyId && 
            x.IsActive);

        if (configuration == null)
        {
            return null;
        }

        var task = await _taskRepository.FirstOrDefaultAsync(x =>
            x.MessageConfigurationId == configuration.Id &&
            x.TaskType == taskType &&
            x.IsActive);

        return task?.Behavior;
    }

    public async Task<bool> ShouldSendMessageAsync(Guid companyId, MessageTaskType taskType)
    {
        var behavior = await GetMessageBehaviorAsync(companyId, taskType);
        
        return behavior switch
        {
            MessageBehavior.AlwaysSend => true,
            MessageBehavior.NeverSend => false,
            MessageBehavior.AskBeforeSend => true, // Bu durumda UI'da kullanıcıya sorulmalı
            _ => false
        };
    }

    public async Task<bool> RequiresConfirmationAsync(Guid companyId, MessageTaskType taskType)
    {
        var behavior = await GetMessageBehaviorAsync(companyId, taskType);
        return behavior == MessageBehavior.AskBeforeSend;
    }
} 