using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class GetMessageUserListFilterDto : PagedAndSortedResultRequestDto
{
    public bool? IsActive { get; set; }
} 