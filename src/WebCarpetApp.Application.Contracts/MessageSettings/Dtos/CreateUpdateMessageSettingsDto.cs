using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.MessageSettings.Dtos;

public class CreateUpdateMessageSettingsDto
{
    [Required]
    public Guid MessageUserId { get; set; }

    public bool UponReceiptMessage { get; set; }
    public bool NewOrderMessage { get; set; }
    public bool WhenDeliveredMessage { get; set; }
    public bool SendUponReceiptMessage { get; set; }
    public bool SendNewOrderMessage { get; set; }
    public bool SendWhenDeliveredMessage { get; set; }
} 