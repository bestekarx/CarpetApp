using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using WebCarpetApp.Messaging.Dtos;

namespace WebCarpetApp.Messaging;

public class MessageUserAppService :
    CrudAppService<
        MessageUser,
        MessageUserDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageUserDto,
        CreateUpdateMessageUserDto>,
    IMessageUserAppService
{
    private readonly IRepository<MessageUser, Guid> _repository;

    public MessageUserAppService(IRepository<MessageUser, Guid> repository)
        : base(repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<MessageUserDto>> GetFilteredListAsync(GetMessageUserListFilterDto input)
    {
        try
        {
            var queryable = await _repository.GetQueryableAsync();
        
            if (input.Active.HasValue)
            {
                queryable = queryable.Where(x => x.Active == input.Active.Value);
            }
        
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var items = await AsyncExecuter.ToListAsync(queryable);

            var dtos = ObjectMapper.Map<List<MessageUser>, List<MessageUserDto>>(items);
            return new PagedResultDto<MessageUserDto>(totalCount, dtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected override MessageUser MapToEntity(CreateUpdateMessageUserDto createInput)
    {
        var entity = base.MapToEntity(createInput);
        entity.TenantId = CurrentTenant.Id;
        return entity;
    }

    protected override void MapToEntity(CreateUpdateMessageUserDto updateInput, MessageUser entity)
    {
        base.MapToEntity(updateInput, entity);
        entity.Active = updateInput.Active;
    }

    protected override async Task<MessageUser> GetEntityByIdAsync(Guid id)
    {
        var messageUser = await base.GetEntityByIdAsync(id);
        if (messageUser == null || !messageUser.Active)
        {
            throw new UserFriendlyException(L["MessageUserNotFound"]);
        }
        return messageUser;
    }
} 