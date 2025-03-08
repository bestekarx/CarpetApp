using System;

namespace WebCarpetApp.Receiveds.Dtos;

public class UpdateReceivedLocationDto
{
    public Guid Id { get; set; }
    public string Coordinate { get; set; }
}