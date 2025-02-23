using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Receiveds.Dtos;

public class UpdateReceivedOrderDto
{
    [Required]
    public List<Guid> OrderedIds { get; set; }
} 