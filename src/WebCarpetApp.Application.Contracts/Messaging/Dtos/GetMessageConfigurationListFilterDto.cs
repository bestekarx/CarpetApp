using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class GetMessageConfigurationListFilterDto : PagedAndSortedResultRequestDto
{
  public string Name { get; set; }
  public bool? Active { get; set; }
}