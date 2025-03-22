using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Permissions;
using WebCarpetApp.Products.Dtos;

namespace WebCarpetApp.Products;

[RemoteService(IsEnabled = true)]
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
    private readonly IRepository<Product, Guid> _repository;

    public ProductAppService(IRepository<Product, Guid> repository)
        : base(repository)
    {
        _repository = repository;
        GetPolicyName = WebCarpetAppPermissions.Products.Default;
        GetListPolicyName = WebCarpetAppPermissions.Products.Default;
        CreatePolicyName = WebCarpetAppPermissions.Products.Create;
        UpdatePolicyName = WebCarpetAppPermissions.Products.Edit;
        DeletePolicyName = WebCarpetAppPermissions.Products.Delete;
    }

    public async Task<PagedResultDto<ProductDto>> GetFilteredListAsync(GetProductListFilterDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        
        // Apply filters
        if (!string.IsNullOrEmpty(input.Name))
        {
            queryable = queryable.Where(x => x.Name.Contains(input.Name));
        }
        
        if (input.Type.HasValue)
        {
            queryable = queryable.Where(x => x.ProductType == input.Type.Value);
        }
        
        if (input.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.Active == input.IsActive.Value);
        }
        
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var orderBy = !string.IsNullOrWhiteSpace(input.Sorting)
            ? input.Sorting
            : "CreationTime desc";
        
        queryable = queryable.OrderBy(orderBy);

        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var items = await AsyncExecuter.ToListAsync(queryable);

        var dtos = ObjectMapper.Map<List<Product>, List<ProductDto>>(items);

        return new PagedResultDto<ProductDto>(totalCount, dtos);
    }

    protected override Task CheckCreatePolicyAsync()
    {
        // Politika kontrolünü atla
        return Task.CompletedTask;
    }
    
    protected override Product MapToEntity(CreateUpdateProductDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 