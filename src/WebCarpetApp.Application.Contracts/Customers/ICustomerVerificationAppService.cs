using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Customers.Dtos;
namespace WebCarpetApp.Customers;

public interface ICustomerVerificationAppService :
ICrudAppService<
    CustomerVerificationDto,
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateCustomerVerificationDto,
    CreateUpdateCustomerVerificationDto>
{
} 