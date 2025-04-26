namespace CarpetApp.Models;

public class SmsUsersModel
{
  public Guid Id { get; set; }
  public bool Active { get; set; }
  public string UserName { get; set; }
  public string Password { get; set; }
  public string Title { get; set; }
}