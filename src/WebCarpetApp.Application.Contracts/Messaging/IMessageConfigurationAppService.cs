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
} 