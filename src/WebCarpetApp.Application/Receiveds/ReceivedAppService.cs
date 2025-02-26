using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using WebCarpetApp.Customers;
using WebCarpetApp.Receiveds.Dtos;

namespace WebCarpetApp.Receiveds;

[RemoteService(IsEnabled = true)]
public class ReceivedAppService : WebCarpetAppAppService, IReceivedAppService
{
    private readonly IRepository<Received, Guid> _repository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ReceivedManager _receivedManager;
    private readonly IRepository<Customer, Guid> _customerRepository;

    public ReceivedAppService(
        IRepository<Received, Guid> repository, 
        IUnitOfWorkManager unitOfWorkManager,
        ReceivedManager receivedManager)
    {
        _repository = repository;
        _unitOfWorkManager = unitOfWorkManager;
        _receivedManager = receivedManager;
    }
    
    public async Task<ReceivedDto> GetAsync(Guid id)
    {
        var Received = await _repository.GetAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(Received);
    }

    public async Task<PagedResultDto<ReceivedDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResultDto<ReceivedDto>> GetFilteredListAsync(GetReceivedListFilterDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        
        if (input.Status.HasValue)
        {
            queryable = queryable.Where(x => x.Status == input.Status.Value);
        }

        if (input.StartDate.HasValue && input.EndDate.HasValue)
        {
            if (input.DateFilterType?.ToLower() == "purchasedate")
            {
                queryable = queryable.Where(x => x.PurchaseDate >= input.StartDate && x.PurchaseDate <= input.EndDate);
            }
            else if (input.DateFilterType?.ToLower() == "receiveddate")
            {
                queryable = queryable.Where(x => x.ReceivedDate >= input.StartDate && x.ReceivedDate <= input.EndDate);
            }
        }

        if (input.CustomerId.HasValue)
        {
            queryable = queryable.Where(x => x.CustomerId == input.CustomerId.Value);
        }

        if (input.VehicleId.HasValue)
        {
            queryable = queryable.Where(x => x.VehicleId == input.VehicleId.Value);
        }

        var query = queryable
            .OrderBy(!string.IsNullOrWhiteSpace(input.Sorting) ? input.Sorting : "CreationTime DESC")
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var receiveds = await AsyncExecuter.ToListAsync<Received>(query);
        var totalCount = await AsyncExecuter.CountAsync<Received>(queryable);

        return new PagedResultDto<ReceivedDto>(
            totalCount,
            ObjectMapper.Map<List<Received>, List<ReceivedDto>>(receiveds)
        );
    }

    public async Task<ReceivedDto> CreateAsync(CreateUpdateReceivedDto input)
    {
        var Received = ObjectMapper.Map<CreateUpdateReceivedDto, Received>(input);
        await _repository.InsertAsync(Received);
        return ObjectMapper.Map<Received, ReceivedDto>(Received);
    }

    public async Task<ReceivedDto> UpdateAsync(Guid id, CreateUpdateReceivedDto input)
    {
        var Received = await _repository.GetAsync(id);
        ObjectMapper.Map(input, Received);
        await _repository.UpdateAsync(Received);
        return ObjectMapper.Map<Received, ReceivedDto>(Received);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
    
    public async Task<ReceivedDto> GetByIdAsync(Guid id)
    {
        var received = await _repository.GetAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    public async Task<ReceivedDto> CancelReceivedAsync(Guid id)
    {   
        var received = await _receivedManager.CancelReceivedAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    public async Task<ReceivedDto> TestReceivedAsync(ReceivedDto model)
    {   
        var received = ObjectMapper.Map<ReceivedDto, Received>(model);
        await _repository.InsertAsync(received);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    public async Task UpdateOrderAsync(UpdateReceivedOrderDto input)
    {
        await _receivedManager.ReorderReceivedItemsAsync(input.OrderedIds);
    }
    
    public async Task<ReceivedDto> CreateWithSmsAsync(CreateReceivedWithSmsDto input)
    {
        // 1. Müşteri varlığını kontrol et
        var customer = await _customerRepository.GetAsync(input.CustomerId);
            
        // 2. Received oluştur ve gerekiyorsa SMS gönder
        var received = await _receivedManager.CreateReceivedAsync(
            input.VehicleId,
            input.CustomerId,
            input.Note,
            input.RowNumber,
            input.PurchaseDate,
            input.SendSms,
            input.CultureCode
        );
            
        // 3. Sonucu DTO'ya dönüştür ve döndür
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }
        
    public async Task<bool> SendReceivedNotificationAsync(Guid receivedId)
    {
        // 1. Received kaydını bul
        var received = await _repository.GetAsync(receivedId);
            
        // 2. Customer bilgilerini getir
        var customer = await _customerRepository.GetAsync(received.CustomerId);
            
        // 3. SMS gönderme işlemini başlat
        await _receivedManager.SendReceivedNotificationAsync(received, customer);
            
        return true;
    }
} 