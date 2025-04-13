using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using WebCarpetApp.Areas.Dtos;

namespace WebCarpetApp.Areas;

public class AreaAppService(IRepository<Area, Guid> repository) :
    CrudAppService<
        Area,
        AreaDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAreaDto,
        CreateUpdateAreaDto>(repository),
    IAreaAppService
{
    public async Task<PagedResultDto<AreaDto>> GetFilteredListAsync(GetAreaListFilterDto input)
    {
        var queryable = await repository.GetQueryableAsync();
        
        if (!string.IsNullOrEmpty(input.Name))
        {
            queryable = queryable.Where(x => x.Name.Contains(input.Name));
        }
        
        if (input.Active.HasValue)
        {
            queryable = queryable.Where(x => x.Active == input.Active.Value);
        }
        
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var orderBy = !string.IsNullOrWhiteSpace(input.Sorting)
            ? input.Sorting
            : "CreationTime desc";
        
        queryable = queryable.OrderBy(orderBy);

        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);
        var items = await AsyncExecuter.ToListAsync(queryable);

        var dtos = ObjectMapper.Map<List<Area>, List<AreaDto>>(items);
        return new PagedResultDto<AreaDto>(totalCount, dtos);
    }

    protected override Area MapToEntity(CreateUpdateAreaDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 