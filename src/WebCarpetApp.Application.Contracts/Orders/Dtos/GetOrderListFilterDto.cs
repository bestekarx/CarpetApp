using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Orders.Dtos;

public class GetOrderListFilterDto : PagedAndSortedResultRequestDto
{
    public OrderStatus? OrderStatus { get; set; }
    public bool? Active { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? ReceivedId { get; set; }
    public Guid? CustomerId { get; set; }
}