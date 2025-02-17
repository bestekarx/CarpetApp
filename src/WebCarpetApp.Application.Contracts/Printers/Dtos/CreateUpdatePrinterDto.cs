using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Printers.Dtos;

public class CreateUpdatePrinterDto
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; }

    [Required]
    [StringLength(20)]
    public string MacAddress { get; set; }
} 