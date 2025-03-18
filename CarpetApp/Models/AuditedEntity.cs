namespace CarpetApp.Models;

public record class AuditedEntity
{
    public bool IsActive { get; set; }
    public DateTime CreationTime { get; protected set; }
    public Guid? CreatorId { get; protected set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId { get; set; }
    public Guid Id { get; set; }
}