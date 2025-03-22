using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Orders.Dtos;

public class CreateUpdateOrderDto
{
    public Guid? ReceivedId { get; set; }

    public int OrderDiscount { get; set; }

    [Required]
    public decimal OrderAmount { get; set; }

    [Required]
    public decimal OrderTotalPrice { get; set; }

    [Required]
    public int OrderStatus { get; set; }

    public int OrderRowNumber { get; set; }

    public bool Active { get; set; }

    public bool CalculatedUsed { get; set; }

    public Guid? UpdatedUserId { get; set; }
} 