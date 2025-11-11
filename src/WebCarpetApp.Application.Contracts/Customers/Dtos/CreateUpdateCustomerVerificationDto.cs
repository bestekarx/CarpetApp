using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Customers.Dtos;

public class CreateUpdateCustomerVerificationDto
{
    public Guid CustomerId { get; set; }
    [Required]
    public string VerificationCode { get; set; } = default!;
    public bool IsUsed { get; set; }
    public DateTime ExpirationTime { get; set; }
}