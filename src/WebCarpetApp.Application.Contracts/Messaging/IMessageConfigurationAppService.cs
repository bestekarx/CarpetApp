using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Messaging.Dtos;

namespace WebCarpetApp.Messaging;

public interface IMessageConfigurationAppService :
    ICrudAppService<
        MessageConfigurationDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageConfigurationDto>
{
    Task<MessageConfigurationDto> AddMessageTaskAsync(Guid id, CreateUpdateMessageTaskDto input);
    Task<MessageConfigurationDto> UpdateMessageTaskAsync(Guid id, Guid taskId, CreateUpdateMessageTaskDto input);
    Task<MessageConfigurationDto> RemoveMessageTaskAsync(Guid id, Guid taskId);
} 