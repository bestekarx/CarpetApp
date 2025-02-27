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
    public int RowNumber { get; set; }
        
    public DateTime? PurchaseDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}