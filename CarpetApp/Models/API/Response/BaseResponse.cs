namespace CarpetApp.Models.API.Response;

public class BaseResponse<TResult>
{
  public bool Success { get; set; }
  public string Message { get; set; }
  public string Exception { get; set; }
  public TResult Result { get; set; }
  public IEnumerable<TResult> ResultList { get; set; }
}