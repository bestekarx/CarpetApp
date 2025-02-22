using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WebCarpetApp.Receiveds.Dtos;

namespace WebCarpetApp.Receiveds;

public interface IReceivedAppService : IApplicationService
{
    Task<ReceivedDto> GetByIdAsync(Guid id);
    Task<ReceivedDto> TestReceivedAsync(ReceivedDto model);
} 