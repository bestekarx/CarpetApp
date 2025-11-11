using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Settings.Dtos;

public class UpdateFicheNoSettingsDto
{
    [StringLength(10)]
    public string Prefix { get; set; }
    
    [Range(0, int.MaxValue)]
    public int StartNumber { get; set; }
} 