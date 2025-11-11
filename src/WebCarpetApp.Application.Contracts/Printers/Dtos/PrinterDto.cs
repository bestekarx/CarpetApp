using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Printers.Dtos;

public class PrinterDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string MacAddress { get; set; }
} 