using System.Diagnostics;

namespace CarpetApp.Service;

public class CarpetExceptionLogger
{
  public static CarpetExceptionLogger Instance { get; } = new();

  public void CrashLog(Exception exception, Dictionary<string, string> properties = null, string customError = null,
    bool globalException = false)
  {
    try
    {
      if (properties is null)
        properties = new Dictionary<string, string>();

      var methodName = new StackTrace(exception).GetFrame(0).GetMethod().Name;
      var pageName = new StackTrace(exception).GetFrame(0).GetMethod().DeclaringType.FullName;
      /*
      properties.Add("JWTToken", !String.IsNullOrWhiteSpace(GlobalSetting.Instance.JWTToken) ? GlobalSetting.Instance.JWTToken : "");

      if (GlobalSetting.Instance.User != null)
      {
          properties.Add("UserToken", GlobalSetting.Instance.User.Token);
          properties.Add("FleetId", GlobalSetting.Instance.User.FleetGuid);
          properties.Add("Userid", GlobalSetting.Instance.User.UserId.ToString());
          properties.Add("Email", GlobalSetting.Instance.User.EMail);
      }

      properties.Add("DeviceId", !String.IsNullOrWhiteSpace(GlobalSetting.Instance.DeviceGuid) ? GlobalSetting.Instance.DeviceGuid : "");
      properties.Add("Time", DateTime.Now.ToString());
      properties.Add("MethodName", methodName);
      properties.Add("PageName", pageName);
      properties.Add("ExceptionMessage", exception.Message);
      properties.Add("ExceptionTrace", exception.StackTrace);
      properties.Add("GlobalException", globalException ? "1" : "0");
      SendTrackError(exception, properties);
      */
    }
    catch (Exception ex)
    {
      // ignored
    }
  }
}