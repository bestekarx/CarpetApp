namespace CarpetApp.Models.API.Response;

public class AuthenticationResponseModel
{
  public UserModel User { get; set; }
  public string AuthToken { get; set; }
}