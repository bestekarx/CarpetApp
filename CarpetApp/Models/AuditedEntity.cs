namespace CarpetApp.Models;

public class AuditedEntity
{
    public bool Active { get; set; }
    public DateTime CreationTime { get; protected set; }
    public Guid? CreatorId { get; protected set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId { get; set; }
    public Guid Id { get; set; }
}