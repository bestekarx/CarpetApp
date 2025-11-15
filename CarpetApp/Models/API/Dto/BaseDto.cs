namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Base DTO with common audit fields (ABP Framework standard)
/// </summary>
public abstract class AuditedEntityDto
{
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId { get; set; }
}
