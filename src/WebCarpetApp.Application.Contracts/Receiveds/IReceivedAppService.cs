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
    Task<ReceivedDto> GetByIdAsync(Guid id);
    Task<ReceivedDto> CancelReceivedAsync(Guid id);
    Task UpdateOrderAsync(UpdateReceivedOrderDto input);
} 