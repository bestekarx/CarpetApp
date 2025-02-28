namespace WebCarpetApp.Orders;

public enum OrderStatus
{
    Passive = 0,
    Active = 1,
    InProcess = 2,  // Halılar yıkama sürecinde
    Completed = 3,  // Yıkama tamamlandı
    ReadyForDelivery = 4, // Teslimat için hazır
    Delivered = 5,  // Teslim edildi
    Cancelled = 6   // İptal edildi
}