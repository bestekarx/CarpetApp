using System;

namespace WebCarpetApp.Receiveds.Dtos;

public class UpdateReceivedDto
{
    public string Note { get; set; }
    public int RowNumber { get; set; }
    public DateTime? PickupDate { get; set; }
}