using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        
        if (input.Active.HasValue)
        {
            queryable = queryable.Where(x => x.Active == input.Active.Value);
        }

        if (input.StartDate.HasValue && input.EndDate.HasValue)
        {
            if (input.DateFilterType == "PickupDate")
            {
                queryable = queryable.Where(x => x.PickupDate >= input.StartDate.Value && x.PickupDate <= input.EndDate.Value);
            }
            else if (input.DateFilterType == "DeliveryDate")
            {
                queryable = queryable.Where(x => x.DeliveryDate >= input.StartDate.Value && x.DeliveryDate <= input.EndDate.Value);
            }
            else
            {
                // Default behavior if no specific date type is specified
                queryable = queryable.Where(x => x.CreationTime >= input.StartDate.Value && x.CreationTime <= input.EndDate.Value);
            }
        }

        if (input.VehicleId.HasValue)
        {
            queryable = queryable.Where(x => x.VehicleId == input.VehicleId.Value);
        }
        
        if (input.CustomerId.HasValue)
        {
            queryable = queryable.Where(x => x.CustomerId == input.CustomerId.Value);
        }
        
        if (!string.IsNullOrEmpty(input.FicheNo))
        {
            queryable = queryable.Where(x => x.FicheNo.Contains(input.FicheNo));
        }
        
        if (input.Type.HasValue)
        {
            queryable = queryable.Where(x => x.Type == input.Type.Value);
        }
        
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var orderBy = !string.IsNullOrWhiteSpace(input.Sorting)
            ? input.Sorting
            : "RowNumber asc";
        
        queryable = queryable.OrderBy(orderBy);

        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var items = await AsyncExecuter.ToListAsync(queryable);

        var dtos = ObjectMapper.Map<List<Received>, List<ReceivedDto>>(items);

        return new PagedResultDto<ReceivedDto>(totalCount, dtos);
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

    public async Task<bool> UpdateLocationReceivedAsync(UpdateReceivedLocationDto updateReceivedLocationDto)
    {
        var received = await _repository.GetAsync(updateReceivedLocationDto.Id);
        var customer = await _customerRepository.GetAsync(received.CustomerId);

        customer.Coordinate = updateReceivedLocationDto.Coordinate;
        
        await _customerRepository.UpdateAsync(customer);
        
        return true;
    }

    public async Task<ReceivedDto> CreateAsync(CreateUpdateReceivedDto input)
    {
        // Burada try-catch kullanmaya devam ediyorum çünkü ReceivedManager'ı çağırıyoruz
        // ve bu muhtemelen karmaşık bir işlemdir
        try
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
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to create received");
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.ReceivedCreationFailed,
                "Received creation failed: " + ex.Message);
        }
    }

    public async Task<ReceivedDto> UpdateAsync(Guid id, CreateUpdateReceivedDto input)
    {
        var received = await _repository.GetAsync(id);
        ObjectMapper.Map(input, received);
        await _repository.UpdateAsync(received);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    public async Task<bool> UpdateCancelReceivedAsync(Guid id)
    {   
        // Burada try-catch kullanmaya devam ediyorum çünkü ReceivedManager'ı çağırıyoruz
        try
        {
            return await _receivedManager.CancelReceivedAsync(id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to cancel received with ID: {Id}", id);
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Failed to cancel received: " + ex.Message);
        }
    }

    public async Task<bool> UpdateReceivedSortListAsync(UpdateReceivedOrderDto input)
    {
        try
        {
            return await _receivedManager.ReorderReceivedItemsAsync(input.OrderedIds);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update received sort list");
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Failed to update received sort list: " + ex.Message);
        }
    }
    
    public async Task<bool> UpdateOrderAsync(UpdateReceivedOrderDto input)
    {
        return await UpdateReceivedSortListAsync(input);
    }

    public async Task<bool> SendReceivedNotificationAsync(Guid receivedId)
    {
        // Burada try-catch kullanmaya devam ediyorum çünkü ReceivedManager'ı çağırıyoruz
        try
        {
            var received = await _repository.GetAsync(receivedId);
            var customer = await _customerRepository.GetAsync(received.CustomerId);
            await _receivedManager.SendReceivedNotificationAsync(received, customer);
                
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to send notification for received ID: {Id}", receivedId);
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.InvalidOperation,
                "Failed to send notification: " + ex.Message);
        }
    }
} 