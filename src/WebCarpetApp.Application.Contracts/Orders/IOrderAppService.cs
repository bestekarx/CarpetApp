using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WebCarpetApp.Orders.Dtos;

namespace WebCarpetApp.Orders;

public interface IOrderAppService : IApplicationService
{
    // Özel servis metodları buraya eklenecek
    Task<OrderDto> GetByIdAsync(Guid id);
} 