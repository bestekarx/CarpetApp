using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.MessageLogs.Dtos;

public class CreateUpdateMessageLogDto
{
    public Guid? UserId { get; set; }
    public Guid? CustomerId { get; set; }

    [StringLength(256)]
    public string? CustomerName { get; set; }

    [Required]
    public string MessageContent { get; set; }

    public bool MessageSuccessfullySend { get; set; }

    [StringLength(20)]
    public string? MessagedPhone { get; set; }
} 