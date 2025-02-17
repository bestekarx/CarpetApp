using System;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Models;
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
        GetPolicyName = WebCarpetAppPermissions.Customers.Default;
        GetListPolicyName = WebCarpetAppPermissions.Customers.Default;
        CreatePolicyName = WebCarpetAppPermissions.Customers.Create;
        UpdatePolicyName = WebCarpetAppPermissions.Customers.Edit;
        DeletePolicyName = WebCarpetAppPermissions.Customers.Delete;
    }

    protected override Customer MapToEntity(CreateUpdateCustomerDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 