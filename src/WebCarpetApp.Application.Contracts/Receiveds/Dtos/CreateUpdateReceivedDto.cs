using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Receiveds.Dtos;

public class CreateUpdateReceivedDto
{
    [Required]
    public Guid VehicleId { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public int Status { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public int RowNumber { get; set; }

    public bool Active { get; set; }

    [Required]
    public DateTime PurchaseDate { get; set; }

    [Required]
    public DateTime ReceivedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedUserId { get; set; }
} 