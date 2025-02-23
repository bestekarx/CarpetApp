using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Messaging.Dtos;

namespace WebCarpetApp.Messaging;

public interface IMessageTemplateAppService :
    ICrudAppService<
        MessageTemplateDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageTemplateDto>
{
    Task<string> FormatMessageAsync(Guid id, Dictionary<string, object> values);
    Task<MessageTemplateDto> GetByTaskTypeAsync(MessageTaskType taskType, string cultureCode = "tr-TR");
} 