using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Invoices.Dtos;
using WebCarpetApp.Permissions;

namespace WebCarpetApp.Invoices;

[Authorize(WebCarpetAppPermissions.Invoices.Default)]
public class InvoiceAppService : WebCarpetAppAppService, IInvoiceAppService
{
    private readonly IRepository<Invoice, Guid> _invoiceRepository;

    public InvoiceAppService(IRepository<Invoice, Guid> invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<InvoiceDto> GetByIdAsync(Guid id)
    {
        var invoice = await _invoiceRepository.GetAsync(id);
        return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
    }
} 