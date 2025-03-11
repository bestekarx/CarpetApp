namespace CarpetApp.Models.API.Request;

public class RequestLoginModel
{
    public string UserNameOrEmailAddress { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = true;
}