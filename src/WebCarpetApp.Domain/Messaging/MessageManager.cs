using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace WebCarpetApp.Messaging
{
    public class MessageManager : DomainService
    {
        private readonly IRepository<MessageConfiguration, Guid> _configurationRepository;
        private readonly IRepository<MessageTask, Guid> _taskRepository;
        private readonly IRepository<MessageTemplate, Guid> _templateRepository;

        public MessageManager(
            IRepository<MessageConfiguration, Guid> configurationRepository,
            IRepository<MessageTask, Guid> taskRepository,
            IRepository<MessageTemplate, Guid> templateRepository)
        {
            _configurationRepository = configurationRepository;
            _taskRepository = taskRepository;
            _templateRepository = templateRepository;
        }

        public async Task<MessageBehavior?> GetMessageBehaviorAsync(Guid companyId, MessageTaskType taskType)
        {
            var configuration = await _configurationRepository.FirstOrDefaultAsync(x => 
                x.CompanyId == companyId && 
                x.Active);

            if (configuration == null)
            {
                return null;
            }

            var task = await _taskRepository.FirstOrDefaultAsync(x =>
                x.MessageConfigurationId == configuration.Id &&
                x.TaskType == taskType &&
                x.Active);

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
        
        public async Task<string> FormatMessageAsync(
            Guid companyId, 
            MessageTaskType taskType, 
            Dictionary<string, object> values, 
            string cultureCode = "tr-TR")
        {
            // 1. İlgili görev tipi için mesaj şablonunu getir
            var template = await _templateRepository.FirstOrDefaultAsync(
                t => t.TaskType == taskType && 
                     t.Active &&
                     t.CultureCode == cultureCode);
            
            if (template == null)
            {
                // Belirtilen dilde şablon yoksa varsayılan Türkçe şablonu dene
                template = await _templateRepository.FirstOrDefaultAsync(
                    t => t.TaskType == taskType && 
                         t.Active &&
                         t.CultureCode == "tr-TR");
                         
                if (template == null)
                {
                    return null; // Şablon bulunamadı
                }
            }
            
            // 2. Şablonu kullanarak mesajı formatla
            return template.FormatMessage(values);
        }
        
        // YENİ METOT: Şirket için MessageUser ID'sini getiren metot
        public async Task<Guid?> GetMessageUserIdAsync(Guid companyId)
        {
            var configuration = await _configurationRepository.FirstOrDefaultAsync(x => 
                x.CompanyId == companyId && 
                x.Active);
                
            return configuration?.MessageUserId;
        }
    }
}