using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace WebCarpetApp.Receiveds;

public class ReceivedManager : DomainService
{
    private readonly IRepository<Received, Guid> _receivedRepository;

    public ReceivedManager(IRepository<Received, Guid> receivedRepository)
    {
        _receivedRepository = receivedRepository;
    }

    public async Task ReorderReceivedItemsAsync(List<Guid> orderedIds)
    {
        var receivedItems = await _receivedRepository.GetListAsync(x => orderedIds.Contains(x.Id));
        
        // Validate that all items exist
        if (receivedItems.Count != orderedIds.Count)
        {
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.EntityNotFound,
                "Some of the received items were not found."
            );
        }

        // Create a dictionary for quick lookup of items
        var itemsDictionary = receivedItems.ToDictionary(x => x.Id);

        // Update row numbers based on the new order
        for (int i = 0; i < orderedIds.Count; i++)
        {
            var item = itemsDictionary[orderedIds[i]];
            item.UpdateRowNumber(i);
        }

        await _receivedRepository.UpdateManyAsync(receivedItems);
    }

    public async Task<Received> CancelReceivedAsync(Guid id)
    {
        var received = await _receivedRepository.GetAsync(id);
        
        if (received == null)
        {
            throw new BusinessException(
                WebCarpetAppDomainErrorCodes.EntityNotFound,
                "Received item not found."
            );
        }

        received.CancelReceive();
        await _receivedRepository.UpdateAsync(received);
        
        return received;
    }
} 