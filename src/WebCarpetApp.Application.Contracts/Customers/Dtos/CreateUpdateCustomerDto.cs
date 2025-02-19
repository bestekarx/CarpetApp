using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Customers.Dtos;

public class CreateUpdateCustomerDto
{
    public Guid AreaId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? UserId { get; set; }
    
    [Required]
    public required string FullName { get; set; }
    
    [Required]
    public required string Phone { get; set; }
    
    [Required]
    public required string CountryCode { get; set; }
    
    [Required]
    public required string Gsm { get; set; }
    
    [Required]
    public required string Address { get; set; }
    
    public string? Coordinate { get; set; }
    public decimal Balance { get; set; }
    public bool Active { get; set; }
    public bool CompanyPermission { get; set; }
    public bool IsConfirmed { get; set; } 
    public DateTime? ConfirmedAt { get; set; }
} 