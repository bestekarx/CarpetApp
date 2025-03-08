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
        ReceivedManager receivedManager,
        IRepository<Customer, Guid> customerRepository)
    {
        _repository = repository;
        _unitOfWorkManager = unitOfWorkManager;
        _receivedManager = receivedManager;
        _customerRepository = customerRepository;
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
            if (input.DateFilterType?.ToLower() == "pickupdate")
            {
                queryable = queryable.Where(x => x.PickupDate >= input.StartDate && x.PickupDate <= input.EndDate);
            }
            else if (input.DateFilterType?.ToLower() == "deliverydate")
            {
                queryable = queryable.Where(x => x.DeliveryDate >= input.StartDate && x.DeliveryDate <= input.EndDate);
            }
        }

        if (input.CustomerId.HasValue)
        {
            queryable = queryable.Where(x => x.CustomerId == input.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(input.FicheNo))
        {
            queryable = queryable.Where(x => x.FicheNo == input.FicheNo);
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

    public async Task<GetByReceivedFilteredItemDto> GetByIdFilteredItemAsync(Guid id)
    {
        var received = await _repository.GetAsync(id);
        var customer = await _customerRepository.GetAsync(received.CustomerId);
        
        var result = new GetByReceivedFilteredItemDto
        {
            Id = received.Id,
            FicheNo = received.FicheNo ?? string.Empty,
            CustomerGsm = customer.Gsm,
            CustomerPhone = customer.Phone,
            RowNumber = received.RowNumber
        };
        
        return result;
    }

    public async Task<bool> UpdateReceivedLocationAsync(UpdateReceivedLocationDto updateReceivedLocationDto)
    {
        var received = await _repository.GetAsync(updateReceivedLocationDto.Id);
        var customer = await _customerRepository.GetAsync(received.CustomerId);
    }

    public async Task<ReceivedDto> CreateAsync(CreateUpdateReceivedDto input)
    {
        var result = await _receivedManager.CreateReceivedAsync(input.VehicleId, 
            input.CustomerId, 
            input.Note,
            (int)input.Type,
            input.RowNumber,
            input.PickupDate,
            input.DeliveryDate);
        
        return ObjectMapper.Map<Received, ReceivedDto>(result);
    }

    public async Task<ReceivedDto> UpdateAsync(Guid id, CreateUpdateReceivedDto input)
    {
        var received = await _repository.GetAsync(id);
        ObjectMapper.Map(input, received);
        await _repository.UpdateAsync(received);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    
    //....
    public async Task<ReceivedDto> CancelReceivedAsync(Guid id)
    {   
        var received = await _receivedManager.CancelReceivedAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    public async Task UpdateReceivedSortListAsync(UpdateReceivedOrderDto input)
    {
        await _receivedManager.ReorderReceivedItemsAsync(input.OrderedIds);
    }
    
    public async Task<bool> SendReceivedNotificationAsync(Guid receivedId)
    {
        var received = await _repository.GetAsync(receivedId);
        var customer = await _customerRepository.GetAsync(received.CustomerId);
        await _receivedManager.SendReceivedNotificationAsync(received, customer);
            
        return true;
    }
    
    
    [RemoteService(IsEnabled = false)]
    public Task UpdateOrderAsync(UpdateReceivedOrderDto input)
    {
        throw new NotImplementedException();
    }

    [RemoteService(IsEnabled = false)]
    public async Task<ReceivedDto> GetAsync(Guid id)
    {
        var Received = await _repository.GetAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(Received);
    }

    [RemoteService(IsEnabled = false)]
    public Task<PagedResultDto<ReceivedDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        throw new NotImplementedException();
    }

    [RemoteService(IsEnabled = false)]
    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
} 