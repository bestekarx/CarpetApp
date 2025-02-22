using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using WebCarpetApp.Receiveds.Dtos;

namespace WebCarpetApp.Receiveds;

[RemoteService(IsEnabled = true)] //or simply [RemoteService(false)]
public class ReceivedAppService : WebCarpetAppAppService, IReceivedAppService
{
    private readonly IRepository<Received, Guid> _receivedRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ReceivedAppService(IRepository<Received, Guid> receivedRepository, IUnitOfWorkManager unitOfWorkManager)
    {
        _receivedRepository = receivedRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task<ReceivedDto> GetByIdAsync(Guid id)
    {
        var received = await _receivedRepository.GetAsync(id);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }

    public async Task<ReceivedDto> TestReceivedAsync(ReceivedDto model)
    {   
        var received = ObjectMapper.Map<ReceivedDto, Received>(model);
        await _receivedRepository.InsertAsync(received);
        return ObjectMapper.Map<Received, ReceivedDto>(received);
    }
 } 