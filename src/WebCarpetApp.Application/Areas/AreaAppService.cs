using System;
using Volo.Abp.Application.Dtos;
using WebCarpetApp.Areas.Dtos;
using WebCarpetApp.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using WebCarpetApp.Permissions;

namespace WebCarpetApp.Areas;

public class AreaAppService : 
    CrudAppService<
        Area,
        AreaDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAreaDto,
        CreateUpdateAreaDto>,
    IAreaAppService
{
    public AreaAppService(IRepository<Area, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebCarpetAppPermissions.Areas.Default;
        GetListPolicyName = WebCarpetAppPermissions.Areas.Default;
        CreatePolicyName = WebCarpetAppPermissions.Areas.Create;
        UpdatePolicyName = WebCarpetAppPermissions.Areas.Edit;
        DeletePolicyName = WebCarpetAppPermissions.Areas.Delete;
    }

    protected override Area MapToEntity(CreateUpdateAreaDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 