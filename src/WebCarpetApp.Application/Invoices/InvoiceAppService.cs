using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Invoices.Dtos;

namespace WebCarpetApp.Invoices;

[RemoteService(IsEnabled = true)]
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
    
    public async Task<PagedResultDto<InvoiceDto>> GetFilteredListAsync(GetInvoiceListFilterDto input)
    {
        var queryable = await _invoiceRepository.GetQueryableAsync();
        
        if (input.OrderId.HasValue)
        {
            queryable = queryable.Where(x => x.OrderId == input.OrderId.Value);
        }

        if (input.CustomerId.HasValue)
        {
            queryable = queryable.Where(x => x.CustomerId == input.CustomerId.Value);
        }
        
        if (input.PaymentType.HasValue)
        {
            queryable = queryable.Where(x => x.PaymentType == input.PaymentType.Value);
        }
        
        if (input.Active.HasValue)
        {
            queryable = queryable.Where(x => x.Active == input.Active.Value);
        }

        var query = queryable
            .OrderBy(!string.IsNullOrWhiteSpace(input.Sorting) ? input.Sorting : "CreationTime DESC")
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var invoices = await AsyncExecuter.ToListAsync<Invoice>(query);
        var totalCount = await AsyncExecuter.CountAsync<Invoice>(queryable);

        return new PagedResultDto<InvoiceDto>(
            totalCount,
            ObjectMapper.Map<List<Invoice>, List<InvoiceDto>>(invoices)
        );
    }

    public async Task<InvoiceDto> CreateAsync(CreateUpdateInvoiceDto input)
    {
        var invoice = new Invoice
        {
            OrderId = input.OrderId,
            UserId = input.UserId,
            CustomerId = input.CustomerId,
            TotalPrice = input.TotalPrice,
            PaidPrice = input.PaidPrice,
            PaymentType = input.PaymentType,
            InvoiceNote = input.InvoiceNote,
            Active = input.Active,
            UpdatedUserId = input.UpdatedUserId
        };
        
        await _invoiceRepository.InsertAsync(invoice);
        return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
    }

    public async Task<InvoiceDto> UpdateAsync(Guid id, CreateUpdateInvoiceDto input)
    {
        var invoice = await _invoiceRepository.GetAsync(id);
        
        invoice.OrderId = input.OrderId;
        invoice.UserId = input.UserId;
        invoice.CustomerId = input.CustomerId;
        invoice.TotalPrice = input.TotalPrice;
        invoice.PaidPrice = input.PaidPrice;
        invoice.PaymentType = input.PaymentType;
        invoice.InvoiceNote = input.InvoiceNote;
        invoice.Active = input.Active;
        invoice.UpdatedUserId = input.UpdatedUserId;
        
        await _invoiceRepository.UpdateAsync(invoice);
        return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _invoiceRepository.DeleteAsync(id);
    }
} 