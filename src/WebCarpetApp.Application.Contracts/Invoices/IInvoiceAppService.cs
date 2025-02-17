using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WebCarpetApp.Invoices.Dtos;

namespace WebCarpetApp.Invoices;

public interface IInvoiceAppService : IApplicationService
{
    Task<InvoiceDto> GetByIdAsync(Guid id);
} 