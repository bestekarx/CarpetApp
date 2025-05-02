using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Permissions;
using WebCarpetApp.Customers.Dtos;

namespace WebCarpetApp.Customers;

public class CustomerAppService :
    CrudAppService<
        Customer,
        CustomerDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCustomerDto,
        CreateUpdateCustomerDto>,
    ICustomerAppService
{
    public CustomerAppService(IRepository<Customer, Guid> repository)
        : base(repository)
    {
    }

    protected override Customer MapToEntity(CreateUpdateCustomerDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 