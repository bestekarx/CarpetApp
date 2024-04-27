using SQLite;

namespace CarpetApp.Entities.Base;

public class Entry
{
    [PrimaryKey]
    [Column("uuid")]
    public Guid Uuid { get; set; } = Guid.NewGuid();
    
    [Column("firm_id")]
    public int FirmId { get; set; }
    
    [Column("created_date")]
    public DateTime CreateDate => DateTime.Now;
    
    [Column("active")]
    public bool Active { get; set; }
    
    [Column("user_id")]
    public int UserId => 1; // login olduğumuz id sabit olarak buraya gelecek...
    
    [Column("updated_user_id")]
    public int UpdatedUserId { get; set; } // login olduğumuz id sabit olarak buraya gelecek. (create ederken burası gitmeyecek.)
    
    [Column("updated_date")]
    public DateTime UpdatedDate { get; set; }
    
    [Column("isSync")]
    public int IsSync { get; set; }
}