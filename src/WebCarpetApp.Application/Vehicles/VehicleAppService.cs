using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Vehicles.Dtos;

namespace WebCarpetApp.Vehicles;

public class VehicleAppService(IRepository<Vehicle, Guid> repository) :
    CrudAppService<
        Vehicle,
        VehicleDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateVehicleDto,
        CreateUpdateVehicleDto>(repository),
    IVehicleAppService
{
    public async Task<PagedResultDto<VehicleDto>> GetFilteredListAsync(GetVehicleListFilterDto input)
    {
        var queryable = await repository.GetQueryableAsync();
        
        // Apply filters
        if (!string.IsNullOrEmpty(input.Name))
        {
            queryable = queryable.Where(x => x.PlateNumber.Contains(input.Name));
            queryable = queryable.Where(x => x.VehicleName.Contains(input.Name));
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

        var dtos = ObjectMapper.Map<List<Vehicle>, List<VehicleDto>>(items);
        return new PagedResultDto<VehicleDto>(totalCount, dtos);
    }
    
    protected override Vehicle MapToEntity(CreateUpdateVehicleDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 