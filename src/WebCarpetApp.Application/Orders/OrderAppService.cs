using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Orders.Dtos;

namespace WebCarpetApp.Orders;

public class OrderAppService : WebCarpetAppAppService, IOrderAppService
{
    private readonly IRepository<Order, Guid> _orderRepository;

    public OrderAppService(IRepository<Order, Guid> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        return ObjectMapper.Map<Order, OrderDto>(order);
    }
} 