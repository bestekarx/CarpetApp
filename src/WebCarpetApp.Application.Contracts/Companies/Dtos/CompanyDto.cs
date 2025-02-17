using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Companies.Dtos;

public class CompanyDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public bool Active { get; set; }
}