using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using WebCarpetApp.Permissions;

namespace WebCarpetApp.Areas;

public class AreaAppService : ApplicationService, IAreaAppService
{
    private readonly IRepository<Area, Guid> _repository;

    public AreaAppService(IRepository<Area, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AreaDto> GetAsync(Guid id)
    {
        var Area = await _repository.GetAsync(id);
        return ObjectMapper.Map<Area, AreaDto>(Area);
    }

    public async Task<PagedResultDto<AreaDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Name" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var Areas = await AsyncExecuter.ToListAsync(query);
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        return new PagedResultDto<AreaDto>(
            totalCount,
            ObjectMapper.Map<List<Area>, List<AreaDto>>(Areas)
        );
    }

    [Authorize(WebCarpetAppPermissions.Areas.Create)]
    public async Task<AreaDto> CreateAsync(CreateUpdateAreaDto input)
    {
        var Area = ObjectMapper.Map<CreateUpdateAreaDto, Area>(input);
        await _repository.InsertAsync(Area);
        return ObjectMapper.Map<Area, AreaDto>(Area);
    }

    [Authorize(WebCarpetAppPermissions.Areas.Edit)]
    public async Task<AreaDto> UpdateAsync(Guid id, CreateUpdateAreaDto input)
    {
        var area = await _repository.GetAsync(id);
        ObjectMapper.Map(input, area);
        await _repository.UpdateAsync(area);
        return ObjectMapper.Map<Area, AreaDto>(area);
    }

    [Authorize(WebCarpetAppPermissions.Areas.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
} 