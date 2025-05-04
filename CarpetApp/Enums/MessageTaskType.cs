namespace CarpetApp.Enums;

public enum MessageTaskType
{
  ReceivedCreated = 10,
  ReceivedCancelled = 11,
  OrderCreated = 20,
  OrderCompleted = 21,
  OrderCancelled = 22,
  InvoiceCreated = 30,

  InvoicePaid = 31
  // Yeni task'ler buraya eklenebilir
}