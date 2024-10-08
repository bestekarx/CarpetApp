namespace CarpetApp.Models;

public record Entry
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public int Id { get; set; }
    public int FirmId { get; set; }
    public DateTime CreatedDate => DateTime.Now;
    public bool Active { get; set; } = true;
    public int UserId { get; set; } = 1;
    public int UpdatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int IsSync { get; set; } = 0; // 0 = işlenmedi gönderilmek için bekliyor. 10 = gönderildi (başarılı).
}