namespace CarpetApp.Helpers;

public class TokenService
{
    public string Token { get; set; }
    public event Action TokenUpdated;

    public void SetToken(string token)
    {
        Token = token;
        TokenUpdated?.Invoke();
    }
}