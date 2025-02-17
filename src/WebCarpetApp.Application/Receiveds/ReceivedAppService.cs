using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Models;
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
} 