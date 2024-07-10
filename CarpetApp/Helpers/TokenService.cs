namespace CarpetApp.Helpers;

public class TokenService
{
    public string? Token { get; private set; }
    public void SetToken(string token) => Token = token;
}