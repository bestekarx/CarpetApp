using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Messaging.Dtos;

namespace WebCarpetApp.Messaging;

public interface IMessageUserAppService :
    ICrudAppService<
        MessageUserDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageUserDto> {}