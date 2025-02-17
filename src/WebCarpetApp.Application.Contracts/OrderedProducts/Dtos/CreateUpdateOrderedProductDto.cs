using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.OrderedProducts.Dtos;

public class CreateUpdateOrderedProductDto
{
    [Required]
    public Guid OrderId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [StringLength(256)]
    public string ProductName { get; set; }

    [Required]
    public decimal ProductPrice { get; set; }

    [Required]
    public int Number { get; set; }

    [Required]
    public int SquareMeter { get; set; }
} 