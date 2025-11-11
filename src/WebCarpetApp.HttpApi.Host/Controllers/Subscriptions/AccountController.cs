using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Controllers.Subscriptions;

[ApiController]
[Route("api/account")]
public class AccountController : AbpControllerBase
{
    private readonly AccountAppService _accountAppService;

    public AccountController(AccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    /// <summary>
    /// Enhanced login with subscription validation
    /// </summary>
    [HttpPost("login")]
    public async Task<LoginWithSubscriptionResultDto> LoginAsync([FromBody] LoginDto input)
    {
        return await _accountAppService.LoginWithSubscriptionAsync(input.UserNameOrEmailAddress, input.Password);
    }

    /// <summary>
    /// Register new tenant with trial subscription
    /// </summary>
    [HttpPost("register-with-trial")]
    public async Task<LoginWithSubscriptionResultDto> RegisterWithTrialAsync([FromBody] CreateTenantWithTrialDto input)
    {
        return await _accountAppService.RegisterWithTrialAsync(input);
    }

    /// <summary>
    /// Get current login status including subscription info
    /// </summary>
    [HttpGet("login-status")]
    public async Task<LoginWithSubscriptionResultDto> GetLoginStatusAsync()
    {
        return await _accountAppService.GetLoginStatusAsync();
    }
}

public class LoginDto
{
    public string UserNameOrEmailAddress { get; set; }
    public string Password { get; set; }
}