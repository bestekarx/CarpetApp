using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Receiveds.Dtos;

public class CreateReceivedWithSmsDto : CreateReceivedDto
{
    public bool SendSms { get; set; } = true;
        
    [StringLength(10)]
    public string CultureCode { get; set; } = "tr-TR";
}