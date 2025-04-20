using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using WebCarpetApp.Companies.Dtos;

namespace WebCarpetApp.Companies;

public class CompanyAppService :
    CrudAppService<
        Company,
        CompanyDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCompanyDto,
        CreateUpdateCompanyDto>,
    ICompanyAppService
{
    private readonly IRepository<Company, Guid> _repository;

    public CompanyAppService(IRepository<Company, Guid> repository)
        : base(repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<CompanyDto>> GetFilteredListAsync(GetCompanyListFilterDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        
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

        var dtos = ObjectMapper.Map<List<Company>, List<CompanyDto>>(items);
        return new PagedResultDto<CompanyDto>(totalCount, dtos);
    }

    protected override Company MapToEntity(CreateUpdateCompanyDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 