using System;

namespace WebCarpetApp.Receiveds.Dtos;

public class GetByReceivedFilteredItemDto
{
    public Guid Id { get; set; }
    public required string FicheNo { get; set; }
    public required string CustomerGsm { get; set; }
    public required string CustomerPhone { get; set; }
    public required string RowNumber { get; set; }
}