using System;
using Volo.Abp.Application.Dtos;
using WebCarpetApp.Companies.Dtos;
using WebCarpetApp.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Permissions;

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
    public CompanyAppService(IRepository<Company, Guid> repository)
        : base(repository)
    {
        GetPolicyName = WebCarpetAppPermissions.Companies.Default;
        GetListPolicyName = WebCarpetAppPermissions.Companies.Default;
        CreatePolicyName = WebCarpetAppPermissions.Companies.Create;
        UpdatePolicyName = WebCarpetAppPermissions.Companies.Edit;
        DeletePolicyName = WebCarpetAppPermissions.Companies.Delete;
    }

    protected override Company MapToEntity(CreateUpdateCompanyDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }
} 