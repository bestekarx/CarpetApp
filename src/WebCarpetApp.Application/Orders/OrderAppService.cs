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
        
        if (input.ReceivedId.HasValue)
        {
            queryable = queryable.Where(x => x.ReceivedId == input.ReceivedId.Value);
        }
        
        if (input.CustomerId.HasValue)
        {
            // İlgili customer'a ait received'ları bul
            var receiveds = await _receivedRepository.GetQueryableAsync();
            var filteredReceiveds = receiveds.Where(r => r.CustomerId == input.CustomerId.Value);
            var receivedIds = filteredReceiveds.Select(r => r.Id).ToList();
            
            // Bu received'larla ilişkili order'ları filtrele
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
        
        // OrderDto'larına ürün ve resim bilgilerini ekleyelim
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
    
    public async Task<bool> UpdateReceivedNoteAsync(UpdateOrderNoteDto updateOrderNoteDto)
    {
        var order = await _orderRepository.GetAsync(updateOrderNoteDto.Id);
        if (!order.ReceivedId.HasValue)
        {
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Order has no associated received record.");
        }
        
        var received = await _receivedRepository.GetAsync(order.ReceivedId.Value);
        received.Note = updateOrderNoteDto.ReceivedNote;
        
        await _receivedRepository.UpdateAsync(received);
        
        return true;
    }
    
    public async Task<bool> UpdateReceivedVehicleAsync(UpdateOrderVehicleDto updateOrderVehicleDto)
    {
        var order = await _orderRepository.GetAsync(updateOrderVehicleDto.Id);
        if (!order.ReceivedId.HasValue)
        {
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Order has no associated received record.");
        }
        
        var received = await _receivedRepository.GetAsync(order.ReceivedId.Value);
        received.VehicleId = updateOrderVehicleDto.VehicleId;
        
        await _receivedRepository.UpdateAsync(received);
        
        return true;
    }
    
    public async Task<bool> UpdateStatusOrderAsync(UpdateOrderStatusDto updateOrderStatusDto)
    {
        try
        {
            var order = await _orderRepository.GetAsync(updateOrderStatusDto.Id);
            order.OrderStatus = updateOrderStatusDto.OrderStatus;
            
            await _orderRepository.UpdateAsync(order);
            
            return true;
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
            var order = await _orderRepository.GetAsync(updateOrderCardDto.Id);
            
            // Update order details
            order.OrderDiscount = updateOrderCardDto.OrderDiscount;
            order.OrderAmount = updateOrderCardDto.OrderAmount;
            
            // Calculate total price - Discount uygulandıktan sonraki toplam fiyat
            order.OrderTotalPrice = updateOrderCardDto.OrderAmount * (1 - (decimal)updateOrderCardDto.OrderDiscount / 100);
            
            await _orderRepository.UpdateAsync(order);
            
            // Existing products'ları sil
            var existingProducts = await _orderedProductRepository.GetListAsync(p => p.OrderId == order.Id);
            foreach (var product in existingProducts)
            {
                await _orderedProductRepository.DeleteAsync(product);
            }
            
            // Yeni products'ları ekle
            var newProducts = ObjectMapper.Map<List<OrderedProductDto>, List<OrderedProduct>>(updateOrderCardDto.Products);
            foreach (var product in newProducts)
            {
                product.OrderId = order.Id;
            }
            await _orderedProductRepository.InsertManyAsync(newProducts);
            
            // Existing images'ları sil
            var existingImages = await _orderImageRepository.GetListAsync(i => i.OrderId == order.Id);
            foreach (var image in existingImages)
            {
                await _orderImageRepository.DeleteAsync(image);
            }
            
            // Yeni images'ları ekle
            var newImages = updateOrderCardDto.Images.Select(i => new OrderImage
            {
                OrderId = order.Id,
                BlobId = i.BlobId
            }).ToList();
            await _orderImageRepository.InsertManyAsync(newImages);
            
            return true;
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