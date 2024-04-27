namespace CarpetApp.Models;

public record class Entry
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public int FirmId { get; set; }
    public DateTime CreateDate => DateTime.Now;
    public bool Active { get; set; }
    public int UserId => 1; 
    public int UpdatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int IsSync { get; set; }
}