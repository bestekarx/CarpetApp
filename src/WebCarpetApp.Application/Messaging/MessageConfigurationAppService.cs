using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebCarpetApp.Messaging.Dtos;
using WebCarpetApp.Permissions;

namespace WebCarpetApp.Messaging;

public class MessageConfigurationAppService :
    CrudAppService<
        MessageConfiguration,
        MessageConfigurationDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageConfigurationDto>,
    IMessageConfigurationAppService
{
    public MessageConfigurationAppService(IRepository<MessageConfiguration, Guid> repository)
        : base(repository)
    {
    }

    protected override async Task<IQueryable<MessageConfiguration>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        return await base.CreateFilteredQueryAsync(input);
    }

    protected override async Task<MessageConfiguration> MapToEntityAsync(CreateUpdateMessageConfigurationDto createInput)
    {
        return new MessageConfiguration(
            GuidGenerator.Create(),
            createInput.CompanyId,
            createInput.MessageUserId,
            createInput.Name,
            createInput.Description
        );
    }

    protected override async Task MapToEntityAsync(CreateUpdateMessageConfigurationDto updateInput, MessageConfiguration entity)
    {
        entity.UpdateMessageUser(updateInput.MessageUserId);
        entity.SetActive(updateInput.Active);
    }
} 