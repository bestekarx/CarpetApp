using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Areas;
using WebCarpetApp.Receiveds.Dtos;

namespace WebCarpetApp.Receiveds;

public interface IReceivedAppService :
    ICrudAppService<
    ReceivedDto,
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateReceivedDto>, IApplicationService
{
    Task<PagedResultDto<ReceivedDto>> GetFilteredListAsync(GetReceivedListFilterDto input);
    Task<GetByReceivedFilteredItemDto> GetByIdFilteredItemAsync(Guid id);
    Task<bool> UpdateLocationReceivedAsync(UpdateReceivedLocationDto updateReceivedLocationDto);
    Task<bool> UpdateCancelReceivedAsync(Guid id);
    Task<bool> UpdateReceivedSortListAsync(UpdateReceivedOrderDto input);
} 