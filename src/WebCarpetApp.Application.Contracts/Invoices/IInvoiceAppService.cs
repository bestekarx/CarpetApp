using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebCarpetApp.Invoices.Dtos;

namespace WebCarpetApp.Invoices;

public interface IInvoiceAppService : IApplicationService
{
    Task<InvoiceDto> GetByIdAsync(Guid id);
    Task<PagedResultDto<InvoiceDto>> GetFilteredListAsync(GetInvoiceListFilterDto input);
    Task<InvoiceDto> CreateAsync(CreateUpdateInvoiceDto input);
    Task<InvoiceDto> UpdateAsync(Guid id, CreateUpdateInvoiceDto input);
    Task DeleteAsync(Guid id);
} 