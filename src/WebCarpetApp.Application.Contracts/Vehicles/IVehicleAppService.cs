using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Vehicles.Dtos;

namespace WebCarpetApp.Vehicles;

public interface IVehicleAppService :
    ICrudAppService<
        VehicleDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateVehicleDto,
        CreateUpdateVehicleDto>
{
    Task<PagedResultDto<VehicleDto>> GetFilteredListAsync(GetVehicleListFilterDto input);
} 