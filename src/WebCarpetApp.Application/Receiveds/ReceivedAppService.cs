using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Permissions;
using WebCarpetApp.Receiveds.Dtos;

namespace WebCarpetApp.Receiveds;

[Authorize(WebCarpetAppPermissions.Receiveds.Default)]
public class ReceivedAppService : WebCarpetAppAppService, IReceivedAppService
{
    private readonly IRepository<Received, Guid> _receivedRepository;

    public ReceivedAppService(IRepository<Received, Guid> receivedRepository)
    {
        _receivedRepository = receivedRepository;
    }

    public async Task<ReceivedDto> GetByIdAsync(Guid id)
    {
        var received = await _receivedRepository.GetAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }
    
    public async Task<ReceivedDto> AddReceived(ReceivedDto model)
    {
        var received = ObjectMapper.Map<ReceivedDto, Received>(model);
        var result = await _receivedRepository.InsertAsync(received);
        return ObjectMapper.Map<Received, ReceivedDto>(result);
    }
} 