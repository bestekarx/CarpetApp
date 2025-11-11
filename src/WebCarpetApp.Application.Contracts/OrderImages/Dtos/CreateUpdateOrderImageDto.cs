using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.OrderImages.Dtos;

public class CreateUpdateOrderImageDto
{
    [Required]
    public Guid OrderId { get; set; }

    [Required]
    [StringLength(500)]
    public string ImagePath { get; set; }

    public DateTime CreatedDate { get; set; }
} 