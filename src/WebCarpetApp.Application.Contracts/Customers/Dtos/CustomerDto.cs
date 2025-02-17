using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Customers.Dtos;

public class CustomerDto : AuditedEntityDto<Guid>
{
    public Guid AreaId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? UserId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string CountryCode { get; set; }
    public string Gsm { get; set; }
    public string Address { get; set; }
    public string? Coordinate { get; set; }
    public decimal Balance { get; set; }
    public bool Active { get; set; }
    public bool CompanyPermission { get; set; }
} 