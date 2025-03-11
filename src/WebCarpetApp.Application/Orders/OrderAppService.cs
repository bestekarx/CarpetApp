using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
using WebCarpetApp.Customers;
using WebCarpetApp.Vehicles;
using WebCarpetApp.Receiveds;

namespace WebCarpetApp.Orders;

[Authorize(WebCarpetAppPermissions.Orders.Default)]
public class OrderAppService : WebCarpetAppAppService, IOrderAppService
{
    private readonly OrderManager _orderManager;
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IRepository<OrderedProduct, Guid> _orderedProductRepository;
    private readonly IRepository<OrderImage, Guid> _orderImageRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;
    private readonly IRepository<Vehicle, Guid> _vehicleRepository;
    private readonly IRepository<Received, Guid> _receivedRepository;

    public OrderAppService(
        OrderManager orderManager,
        IRepository<Order, Guid> orderRepository,
        IRepository<OrderedProduct, Guid> orderedProductRepository,
        IRepository<OrderImage, Guid> orderImageRepository,
        IRepository<Customer, Guid> customerRepository,
        IRepository<Vehicle, Guid> vehicleRepository,
        IRepository<Received, Guid> receivedRepository)
    {
        _orderManager = orderManager;
        _orderRepository = orderRepository;
        _orderedProductRepository = orderedProductRepository;
        _orderImageRepository = orderImageRepository;
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
        _receivedRepository = receivedRepository;
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
    
    public async Task<PagedResultDto<OrderDto>> GetFilteredListAsync(GetOrderListFilterDto input)
    {
        var queryable = await _orderRepository.GetQueryableAsync();
        
        if (input.OrderStatus.HasValue)
        {
            queryable = queryable.Where(x => x.OrderStatus == input.OrderStatus.Value);
        }
        
        if (input.Active.HasValue)
        {
            queryable = queryable.Where(x => x.Active == input.Active.Value);
        }

        if (input.VehicleId.HasValue)
        {
            // İlgili received kayıtlarını bul
            var receiveds = await _receivedRepository.GetQueryableAsync();
            var filteredReceiveds = receiveds.Where(r => r.VehicleId == input.VehicleId.Value);
            var receivedIds = filteredReceiveds.Select(r => r.Id).ToList();
            
            // Bu received'larla ilişkili order'ları filtrele
            queryable = queryable.Where(o => o.ReceivedId.HasValue && receivedIds.Contains(o.ReceivedId.Value));
        }
        
        if (input.CustomerId.HasValue)
        {
            var receiveds = await _receivedRepository.GetQueryableAsync();
            var filteredReceiveds = receiveds.Where(r => r.CustomerId == input.CustomerId.Value);
            var receivedIds = filteredReceiveds.Select(r => r.Id).ToList();
            
            queryable = queryable.Where(o => o.ReceivedId.HasValue && receivedIds.Contains(o.ReceivedId.Value));
        }
        
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var orderBy = !string.IsNullOrWhiteSpace(input.Sorting)
            ? input.Sorting
            : "OrderRowNumber asc";
        
        queryable = queryable.OrderBy(orderBy);

        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var items = await AsyncExecuter.ToListAsync(queryable);

        var dtos = ObjectMapper.Map<List<Order>, List<OrderDto>>(items);
        
        foreach (var dto in dtos)
        {
            var products = await _orderedProductRepository.GetListAsync(p => p.OrderId == dto.Id);
            var images = await _orderImageRepository.GetListAsync(i => i.OrderId == dto.Id);
            
            dto.Products = ObjectMapper.Map<List<OrderedProduct>, List<OrderedProductDto>>(products);
            dto.Images = ObjectMapper.Map<List<OrderImage>, List<OrderImageDto>>(images);
        }

        return new PagedResultDto<OrderDto>(totalCount, dtos);
    }
    
    public async Task<GetByOrderFilteredItemDto> GetByIdFilteredItemAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        if (!order.ReceivedId.HasValue)
        {
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Order has no associated received record.");
        }
        
        var received = await _receivedRepository.GetAsync(order.ReceivedId.Value);
        var customer = await _customerRepository.GetAsync(received.CustomerId);
        
        // Vehicle bilgisini al
        string vehicleName = null;
        Guid? vehicleId = null;

        var vehicle = await _vehicleRepository.GetAsync(received.VehicleId);
        vehicleName = $"{vehicle.VehicleName} - {vehicle.PlateNumber}";
        vehicleId = vehicle.Id;

        var result = new GetByOrderFilteredItemDto
        {
            Id = order.Id,
            ReceivedNote = received.Note,
            CustomerGsm = customer.Gsm,
            VehicleName = vehicleName,
            VehicleId = vehicleId,
            OrderRowNumber = order.OrderRowNumber,
            OrderId = order.Id
        };
        
        return result;
    }
    
    public async Task<bool> UpdateStatusOrderAsync(UpdateOrderStatusDto updateOrderStatusDto)
    {
        try
        {
            return await _orderManager.UpdateOrderStatusAsync(updateOrderStatusDto.Id, updateOrderStatusDto.OrderStatus);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update order status for ID: {Id}", updateOrderStatusDto.Id);
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Failed to update order status: " + ex.Message);
        }
    }
    
    public async Task<OrderCardDto> GetOrderCardListAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        var products = await _orderedProductRepository.GetListAsync(p => p.OrderId == id);
        var images = await _orderImageRepository.GetListAsync(i => i.OrderId == id);
        
        var orderCardDto = new OrderCardDto
        {
            Id = order.Id,
            OrderRowNumber = order.OrderRowNumber,
            OrderStatus = order.OrderStatus,
            OrderTotalPrice = order.OrderTotalPrice,
            OrderDiscount = order.OrderDiscount,
            CreationTime = order.CreationTime,
            LastModificationTime = order.LastModificationTime,
            Products = ObjectMapper.Map<List<OrderedProduct>, List<OrderedProductDto>>(products),
            Images = ObjectMapper.Map<List<OrderImage>, List<OrderImageDto>>(images)
        };
        
        return orderCardDto;
    }
    
    public async Task<bool> UpdateOrderCardListAsync(UpdateOrderCardDto updateOrderCardDto)
    {
        try
        {
            // Convert DTOs to domain entities
            var products = ObjectMapper.Map<List<OrderedProductDto>, List<OrderedProduct>>(updateOrderCardDto.Products);
            var imageIds = updateOrderCardDto.Images.Select(i => i.BlobId).ToList();
            
            return await _orderManager.UpdateOrderCardAsync(
                updateOrderCardDto.Id,
                updateOrderCardDto.OrderDiscount,
                updateOrderCardDto.OrderAmount,
                products,
                imageIds);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update order card for ID: {Id}", updateOrderCardDto.Id);
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Failed to update order card: " + ex.Message);
        }
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
            var orderedProducts = 
                ObjectMapper.Map<List<OrderedProductDto>, List<OrderedProduct>>(input.Products);
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