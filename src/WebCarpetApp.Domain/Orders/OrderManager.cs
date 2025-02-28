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


        /// <summary>
        /// Siparişten resim siler
        /// </summary>
        public async Task RemoveImageFromOrderAsync(Guid orderImageId)
        {
            await _orderImageRepository.DeleteAsync(orderImageId);
        }
    }
} 