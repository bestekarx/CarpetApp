using SQLite;

namespace CarpetApp.Entities.Base;

public class Entry : IEntryComparable
{
    [Column("id")] public int Id { get; set; }

    [Column("firm_id")] public int FirmId { get; set; }

    [Column("created_date")] public DateTime Date { get; set; } = DateTime.Now;
    [Column("active")] public bool Active { get; set; } = true;

    [Column("user_id")] public int UserId { get; set; } = 1; // login olduğumuz id sabit olarak buraya gelecek...

    [Column("updated_user_id")]
    public int
        UpdatedUserId
    {
        get;
        set;
    } // login olduğumuz id sabit olarak buraya gelecek. (create ederken burası gitmeyecek.)

    [Column("is_sync")] public int IsSync { get; set; } // 0 = gönderilmeyi bekliyor. 1 = gönderildi, 

    [PrimaryKey] [Column("uuid")] public Guid Uuid { get; set; } = Guid.NewGuid();

    [Column("updated_date")] public DateTime UpdatedDate { get; set; }
}