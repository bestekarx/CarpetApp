namespace CarpetApp.Models.API.Request;

public class RequestLoginModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public decimal Code { get; set; }
}