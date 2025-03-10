using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;
using WebCarpetApp.Messaging;
using WebCarpetApp.Products;
using WebCarpetApp.Receiveds;

namespace WebCarpetApp.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderedProduct, Guid> _orderedProductRepository;
        private readonly IRepository<OrderImage, Guid> _orderImageRepository;
        private readonly IRepository<Received, Guid> _receivedRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        
        public OrderManager(
            IRepository<Order, Guid> orderRepository,
            IRepository<OrderedProduct, Guid> orderedProductRepository,
            IRepository<OrderImage, Guid> orderImageRepository,
            IRepository<Received, Guid> receivedRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _orderRepository = orderRepository;
            _orderedProductRepository = orderedProductRepository;
            _orderImageRepository = orderImageRepository;
            _receivedRepository = receivedRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<Order> CreateOrder(Order orderModel, List<OrderedProduct> products, List<Guid> images)
        {
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            
            try
            {
                var received = await _receivedRepository.GetAsync(orderModel.ReceivedId ?? Guid.Empty);
                if (received.Status != ReceivedStatus.Active)
                {
                    throw new BusinessException(
                        WebCarpetAppDomainErrorCodes.InvalidOperation,
                        "Received is not active.");
                }
                
                var savedOrder = await _orderRepository.InsertAsync(orderModel);
                await uow.SaveChangesAsync();
    
                foreach (var orderedProduct in products)
                {
                    orderedProduct.OrderId = savedOrder.Id;    
                }
                await _orderedProductRepository.InsertManyAsync(products);
    
                var orderImages = images.Select(blobId => new OrderImage() { 
                    OrderId = savedOrder.Id, 
                    BlobId = blobId 
                }).ToList();
                await _orderImageRepository.InsertManyAsync(orderImages);
    
                await uow.CompleteAsync();
                return savedOrder;
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.OrderCreationFailed,
                    "Order creation failed: " + ex.Message);
            }
        }
        
        public async Task<bool> UpdateReceivedNoteAsync(Guid orderId, string note)
        {
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            
            try
            {
                var order = await _orderRepository.GetAsync(orderId);
                if (!order.ReceivedId.HasValue)
                {
                    throw new BusinessException(
                        WebCarpetAppDomainErrorCodes.InvalidOperation,
                        "Order has no associated received record.");
                }
                
                var received = await _receivedRepository.GetAsync(order.ReceivedId.Value);
                received.UpdateNote(note);
                await _receivedRepository.UpdateAsync(received);
                await uow.CompleteAsync();
                return true;
            }   
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.InvalidOperation,
                    "Failed to update received note: " + ex.Message);
            }
        }
        
        public async Task<bool> UpdateReceivedVehicleAsync(Guid orderId, Guid vehicleId)
        {
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            
            try
            {
                var order = await _orderRepository.GetAsync(orderId);
                if (!order.ReceivedId.HasValue)
                {
                    throw new BusinessException(
                        WebCarpetAppDomainErrorCodes.InvalidOperation,
                        "Order has no associated received record.");
                }
                
                var received = await _receivedRepository.GetAsync(order.ReceivedId.Value);
                received.UpdateVehicle(vehicleId);
                await _receivedRepository.UpdateAsync(received);
                
                await uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.InvalidOperation,
                    "Failed to update received vehicle: " + ex.Message);
            }
        }
        
        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            
            try
            {
                var order = await _orderRepository.GetAsync(orderId);
                order.OrderStatus = status;
                await _orderRepository.UpdateAsync(order);
                await uow.CompleteAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.InvalidOperation,
                    "Failed to update order status: " + ex.Message);
            }
        }
        
        public async Task<bool> UpdateOrderCardAsync(Guid orderId, int orderDiscount, decimal orderAmount, 
            List<OrderedProduct> newProducts, List<Guid> imageIds)
        {
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            
            try
            {
                var order = await _orderRepository.GetAsync(orderId);
                
                order.OrderDiscount = orderDiscount;
                order.OrderAmount = orderAmount;
                
                order.OrderTotalPrice = orderAmount * (1 - (decimal)orderDiscount / 100);
                
                await _orderRepository.UpdateAsync(order);
                
                var existingProducts = await _orderedProductRepository.GetListAsync(p => p.OrderId == order.Id);
                foreach (var product in existingProducts)
                {
                    await _orderedProductRepository.DeleteAsync(product);
                }
                
                foreach (var product in newProducts)
                {
                    product.OrderId = order.Id;
                }
                await _orderedProductRepository.InsertManyAsync(newProducts);
                
                var existingImages = await _orderImageRepository.GetListAsync(i => i.OrderId == order.Id);
                foreach (var image in existingImages)
                {
                    await _orderImageRepository.DeleteAsync(image);
                }
                
                var orderImages = imageIds.Select(blobId => new OrderImage
                {
                    OrderId = order.Id,
                    BlobId = blobId
                }).ToList();
                await _orderImageRepository.InsertManyAsync(orderImages);
                
                await uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                throw new BusinessException(
                    WebCarpetAppDomainErrorCodes.InvalidOperation,
                    "Failed to update order card: " + ex.Message);
            }
        }
    }
} 