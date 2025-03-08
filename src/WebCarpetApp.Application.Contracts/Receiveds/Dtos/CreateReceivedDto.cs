using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Receiveds.Dtos;

public class CreateReceivedDto
{
    [Required]
    public Guid VehicleId { get; set; }
        
    [Required]
    public Guid CustomerId { get; set; }
        
    public string Note { get; set; }
    [Required]
    public ReceivedType Type { get; set; }
        
    [Required]
    public int RowNumber { get; set; }
        
    public DateTime? PickupDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    
    [StringLength(50)]
    public string? FicheNo { get; set; }
}