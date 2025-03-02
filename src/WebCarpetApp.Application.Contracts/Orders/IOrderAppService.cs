using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Orders.Dtos;

namespace WebCarpetApp.Orders;

public interface IOrderAppService : IApplicationService
{
    Task<OrderDto> GetAsync(Guid id);
    //Task<PagedResultDto<OrderDto>> GetFilteredListAsync(GetOrderListFilterDto input);
    Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<OrderDto> CreateAsync(CreateOrderDto input);
    Task<OrderDto> UpdateAsync(Guid id, CreateUpdateOrderDto input);
    Task DeleteAsync(Guid id);
} 