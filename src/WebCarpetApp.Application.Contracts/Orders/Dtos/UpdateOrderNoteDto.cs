using System;

namespace WebCarpetApp.Orders.Dtos
{
    public class UpdateOrderNoteDto
    {
        public Guid Id { get; set; }
        public string ReceivedNote { get; set; }
    }
} 