namespace WebCarpetApp.Messaging;

public enum MessageTaskType
{
    ReceivedCreated = 1,
    ReceivedCancelled = 2,
    OrderCreated = 3,
    OrderCompleted = 4,
    OrderCancelled = 5,
    InvoiceCreated = 6,
    InvoicePaid = 7
    // Yeni task'ler buraya eklenebilir
} 