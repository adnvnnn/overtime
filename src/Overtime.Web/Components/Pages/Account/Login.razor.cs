using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Overtime.Web.Resources;

namespace Overtime.Web.Components.Pages.Account;

public partial class Login
{
    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "step")]
    public string? Step { get; set; }

    [SupplyParameterFromQuery(Name = "phone")]
    public string? Phone { get; set; }

    [SupplyParameterFromQuery(Name = "returnUrl")]
    public string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery(Name = "dev")]
    public string? Dev { get; set; }

    [SupplyParameterFromForm(FormName = "request-otp")]
    public PhoneInputModel PhoneModel { get; set; } = new();

    [SupplyParameterFromForm(FormName = "verify-otp")]
    public OtpInputModel OtpModel { get; set; } = new();

    [SupplyParameterFromForm(FormName = "resend-otp")]
    public ResendInputModel ResendModel { get; set; } = new();

    private string? _error;

    private bool IsVerifyStep => Step == "verify" && !string.IsNullOrEmpty(Phone);

    protected override void OnInitialized()
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
            NavigationManager.NavigateTo(SafeReturnUrl(), replace: true);
    }

    private async Task RequestCode()
    {
        var phone = Overtime.Web.Services.OtpService.NormalizePhone(PhoneModel.Phone);
        if (!Overtime.Web.Services.OtpService.IsValidPhone(phone))
        {
            _error = Localizer["Login_InvalidPhoneError"].Value;
            return;
        }

        await SendCodeAndRedirect(phone);
    }

    private async Task ResendCode()
    {
        var phone = Phone ?? "";
        await SendCodeAndRedirect(phone);
    }

    private async Task SendCodeAndRedirect(string phone)
    {
        var (ok, error, debugCode) = await OtpService.RequestCodeAsync(phone);
        if (!ok)
        {
            _error = error;
            return;
        }

        var url = $"/account/login?step=verify&phone={Uri.EscapeDataString(phone)}&returnUrl={Uri.EscapeDataString(ReturnUrl ?? "/")}";
        if (Env.IsDevelopment() && debugCode is not null)
            url += $"&dev={debugCode}";

        NavigationManager.NavigateTo(url, replace: true);
    }

    private async Task VerifyCode()
    {
        var phone = Phone ?? "";
        var code = Overtime.Web.Services.OtpService.NormalizePhone(OtpModel.Code);

        var (ok, error, user) = await OtpService.VerifyCodeAsync(phone, code);
        if (!ok || user is null)
        {
            _error = error;
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.MobilePhone, user.PhoneNumber),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        NavigationManager.NavigateTo(SafeReturnUrl(), replace: true);
    }

    private string SafeReturnUrl() =>
        !string.IsNullOrEmpty(ReturnUrl) && ReturnUrl.StartsWith('/') && !ReturnUrl.StartsWith("//")
            ? ReturnUrl
            : "/";

    public class PhoneInputModel
    {
        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_LoginPhoneRequired))]
        public string Phone { get; set; } = "";
    }

    public class OtpInputModel
    {
        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_OtpCodeRequired))]
        [StringLength(6, MinimumLength = 6, ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_OtpCodeLength))]
        public string Code { get; set; } = "";
    }

    public class ResendInputModel;
}
