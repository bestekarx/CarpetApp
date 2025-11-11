using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Products.Dtos;

namespace WebCarpetApp.Products;

public interface IProductAppService : 
    ICrudAppService<
        ProductDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProductDto,
        CreateUpdateProductDto>
{
    Task<PagedResultDto<ProductDto>> GetFilteredListAsync(GetProductListFilterDto input);
} 