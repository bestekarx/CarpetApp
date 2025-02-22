using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Areas;

public interface IAreaAppService :
    ICrudAppService< //Defines CRUD methods
        AreaDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateAreaDto> //Used to create/update a book
{
}

/*
public interface IAreaAppService : 
    ICrudAppService<
        AreaDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAreaDto>
{
} */