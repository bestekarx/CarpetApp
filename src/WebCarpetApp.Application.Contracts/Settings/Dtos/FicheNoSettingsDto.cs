using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Settings.Dtos;

public class FicheNoSettingsDto
{
    [StringLength(10)]
    public string Prefix { get; set; }
    
    [Range(0, int.MaxValue)]
    public int StartNumber { get; set; }
    
    public int LastNumber { get; set; }
} 