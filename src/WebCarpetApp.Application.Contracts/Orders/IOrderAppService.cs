using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Orders.Dtos;

namespace WebCarpetApp.Orders;

public interface IOrderAppService : IApplicationService
{
    Task<OrderDto> CreateAsync(CreateOrderDto input);
    Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<OrderDto> GetAsync(Guid id);
    Task<OrderDto> UpdateAsync(Guid id, CreateUpdateOrderDto input);
    Task DeleteAsync(Guid id);
    
    Task<PagedResultDto<OrderDto>> GetFilteredListAsync(GetOrderListFilterDto input);
    Task<GetByOrderFilteredItemDto> GetByIdFilteredItemAsync(Guid id);
    Task<bool> UpdateStatusOrderAsync(UpdateOrderStatusDto updateOrderStatusDto);
    Task<OrderCardDto> GetOrderCardListAsync(Guid id);
    Task<bool> UpdateOrderCardListAsync(UpdateOrderCardDto updateOrderCardDto);
} 