using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Products.Dtos;

public class CreateUpdateProductDto
{
    public Guid UserId { get; set; }
    public decimal Price { get; set; }
    [Required]
    public required string Name { get; set; }
    public int Type { get; set; }
    public bool Active { get; set; }
} 