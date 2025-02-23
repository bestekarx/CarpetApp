using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Areas;

public interface IAreaAppService :
    ICrudAppService< 
        AreaDto, 
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAreaDto>
{
}
