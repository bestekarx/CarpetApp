using System;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Models;
using WebCarpetApp.Permissions;
using WebCarpetApp.Vehicles.Dtos;

namespace WebCarpetApp.Vehicles;

public class VehicleAppService :
    CrudAppService<
        Vehicle,
        VehicleDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateVehicleDto,
        CreateUpdateVehicleDto>,
    IVehicleAppService
{
    public VehicleAppService(IRepository<Vehicle, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebCarpetAppPermissions.Vehicles.Default;
        GetListPolicyName = WebCarpetAppPermissions.Vehicles.Default;
        CreatePolicyName = WebCarpetAppPermissions.Vehicles.Create;
        UpdatePolicyName = WebCarpetAppPermissions.Vehicles.Edit;
        DeletePolicyName = WebCarpetAppPermissions.Vehicles.Delete;
    }

    protected override Vehicle MapToEntity(CreateUpdateVehicleDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 