using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using WebCarpetApp.Areas.Dtos;

namespace WebCarpetApp.Areas;

public interface IAreaAppService :
    ICrudAppService<
        AreaDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAreaDto,
        CreateUpdateAreaDto>
{
    Task<PagedResultDto<AreaDto>> GetFilteredListAsync(GetAreaListFilterDto input);
}
