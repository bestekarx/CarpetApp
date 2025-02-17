using System;
using WebCarpetApp.Companies.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Companies;

public interface ICompanyAppService :
    ICrudAppService<
        CompanyDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCompanyDto,
        CreateUpdateCompanyDto>
{
} 