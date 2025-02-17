using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Permissions;
using WebCarpetApp.Products.Dtos;

namespace WebCarpetApp.Products;

public class ProductAppService :
    CrudAppService<
        Product,
        ProductDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProductDto,
        CreateUpdateProductDto>,
    IProductAppService
{
    public ProductAppService(IRepository<Product, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebCarpetAppPermissions.Products.Default;
        GetListPolicyName = WebCarpetAppPermissions.Products.Default;
        CreatePolicyName = WebCarpetAppPermissions.Products.Create;
        UpdatePolicyName = WebCarpetAppPermissions.Products.Edit;
        DeletePolicyName = WebCarpetAppPermissions.Products.Delete;
    }

    protected override Product MapToEntity(CreateUpdateProductDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 