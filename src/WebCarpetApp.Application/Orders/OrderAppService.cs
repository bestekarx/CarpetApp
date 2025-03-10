using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.OrderedProducts.Dtos;
using WebCarpetApp.OrderImages.Dtos;
using WebCarpetApp.Orders.Dtos;
using WebCarpetApp.Permissions;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace WebCarpetApp.Orders;

[Authorize(WebCarpetAppPermissions.Orders.Default)]
public class OrderAppService : WebCarpetAppAppService, IOrderAppService
{
    private readonly OrderManager _orderManager;
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<OrderedProduct, Guid> _orderedProductRepository;
    private readonly IRepository<OrderImage, Guid> _orderImageRepository;

    public OrderAppService(
        OrderManager orderManager,
        IRepository<Order, Guid> orderRepository,
        IRepository<OrderedProduct, Guid> orderedProductRepository,
        IRepository<OrderImage, Guid> orderImageRepository)
    {
        _orderManager = orderManager;
        _orderRepository = orderRepository;
        _orderedProductRepository = orderedProductRepository;
        _orderImageRepository = orderImageRepository;
    }

    
    public async Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var orders = await _orderRepository.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting ?? "creationTime desc"
        );

        var totalCount = await _orderRepository.CountAsync();
        var dtos = ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
        
        return new PagedResultDto<OrderDto>(totalCount, dtos);
    }
    
    public async Task<OrderDto> GetAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        var orderDto = ObjectMapper.Map<Order, OrderDto>(order);
        
        var products = await _orderedProductRepository.GetListAsync(p => p.OrderId == id);
        var images = await _orderImageRepository.GetListAsync(i => i.OrderId == id);
        
        orderDto.Products = ObjectMapper.Map<List<OrderedProduct>, List<OrderedProductDto>>(products);
        orderDto.Images = ObjectMapper.Map<List<OrderImage>, List<OrderImageDto>>(images);
        
        return orderDto;
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto input)
    {
        try
        {
            var order = ObjectMapper.Map<CreateOrderDto, Order>(input);
            var orderedProducts = ObjectMapper.Map<List<OrderedProductDto>, List<OrderedProduct>>(input.Products);
            var imageIds = input.Images?.Select(i => i.BlobId).ToList() ?? new List<Guid>();
            
            var createdOrder = await _orderManager.CreateOrder(order, orderedProducts, imageIds);
            
            return await GetAsync(createdOrder.Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Order creation failed.");
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.OrderCreationFailed,
                "Sipariş oluşturulurken bir hata meydana geldi.");
        }
    }
   
    public Task<OrderDto> UpdateAsync(Guid id, CreateUpdateOrderDto input)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _orderRepository.DeleteAsync(id);
    }
} 