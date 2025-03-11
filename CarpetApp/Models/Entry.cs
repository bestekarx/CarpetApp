using System.Text.Json.Serialization;

namespace CarpetApp.Models;

public record Entry
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("firm_id")]
    public int FirmId { get; set; }
    [JsonPropertyName("date")]
    public DateTime Date => DateTime.Now;
    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;
    [JsonPropertyName("user_id")]
    public int UserId { get; set; } = 1;
    [JsonPropertyName("updated_user_id")]
    public int UpdatedUserId { get; set; }
    [JsonPropertyName("updated_date")]
    public DateTime UpdatedDate { get; set; } // api tarafından işlenen tarih.
    [JsonPropertyName("is_sync")]
    public int IsSync { get; set; } = 0; // 0 = işlenmedi gönderilmek için bekliyor. 10 = gönderildi (başarılı).
}