using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Areas;
using WebCarpetApp.Receiveds.Dtos;

namespace WebCarpetApp.Receiveds;

public interface IReceivedAppService : IApplicationService
{
    Task<PagedResultDto<ReceivedDto>> GetFilteredListAsync(GetReceivedListFilterDto input);
    Task<GetByReceivedFilteredItemDto> GetByIdFilteredItemAsync(Guid id);
    Task<bool> UpdateLocationReceivedAsync(UpdateReceivedLocationDto updateReceivedLocationDto);
    Task<ReceivedDto> CreateAsync(CreateUpdateReceivedDto input);
    Task<ReceivedDto> UpdateAsync(Guid id, CreateUpdateReceivedDto input);
    Task<bool> UpdateCancelReceivedAsync(Guid id);
    Task<bool> UpdateReceivedSortListAsync(UpdateReceivedOrderDto input);
    Task<bool> SendReceivedNotificationAsync(Guid receivedId);
    Task<bool> UpdateOrderAsync(UpdateReceivedOrderDto input);
} 