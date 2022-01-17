using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.Account.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(LoginModel))]
public class OpenIddictSupportedLoginModel : LoginModel
{
    public OpenIddictSupportedLoginModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IOptions<IdentityOptions> identityOptions)
        : base(
            schemeProvider,
            accountOptions,
            identityOptions)
    {
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        LoginInput = new LoginInputModel();

        var request = ParseReturnUrl(ReturnUrl);

        if (request != null)
        {
            ShowCancelButton = true;

            LoginInput.UserNameOrEmailAddress = request.LoginHint;

            //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
            var tenant = request.GetParameter(TenantResolverConsts.DefaultTenantKey)?.Value?.ToString();
            if (!string.IsNullOrEmpty(tenant))
            {
                CurrentTenant.Change(Guid.Parse(tenant));
                Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
            }
        }

        var providers = await GetExternalProviders();
        ExternalProviders = providers.ToList();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        if (IsExternalLoginOnly)
        {
            return await base.OnPostExternalLogin(providers.First().AuthenticationScheme);
        }

        return Page();
    }

    public override async Task<IActionResult> OnPostAsync(string action)
    {
        var request = ParseReturnUrl(ReturnUrl);
        if (action == "Cancel")
        {
            if (request == null)
            {
                return Redirect("~/");
            }

            return Redirect(ReturnUrl);
        }

        await CheckLocalLoginAsync();

        ValidateModel();

        await IdentityOptions.SetAsync();

        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        await ReplaceEmailToUsernameOfInputIfNeeds();

        var result = await SignInManager.PasswordSignInAsync(
            LoginInput.UserNameOrEmailAddress,
            LoginInput.Password,
            LoginInput.RememberMe,
            true
        );

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = result.ToIdentitySecurityLogAction(),
            UserName = LoginInput.UserNameOrEmailAddress,
            ClientId = request?.ClientId
        });

        if (result.RequiresTwoFactor)
        {
            return await TwoFactorLoginResultAsync();
        }

        if (result.IsLockedOut)
        {
            Alerts.Warning(L["UserLockedOutMessage"]);
            return Page();
        }

        if (result.IsNotAllowed)
        {
            Alerts.Warning(L["LoginIsNotAllowed"]);
            return Page();
        }

        if (!result.Succeeded)
        {
            Alerts.Danger(L["InvalidUserNameOrPassword"]);
            return Page();
        }

        //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
        var user = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                   await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

        Debug.Assert(user != null, nameof(user) + " != null");

        return RedirectSafely(ReturnUrl, ReturnUrlHash);
    }

    public override async Task<IActionResult> OnPostExternalLogin(string provider)
    {
        if (AccountOptions.WindowsAuthenticationSchemeName == provider)
        {
            return await ProcessWindowsLoginAsync();
        }

        return await base.OnPostExternalLogin(provider);
    }

    protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
    {
        var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
        if (result.Succeeded)
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                Items =
                    {
                        {
                            "LoginProvider", AccountOptions.WindowsAuthenticationSchemeName
                        },
                    }
            };

            var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Principal.FindFirstValue(ClaimTypes.PrimarySid)));
            id.AddClaim(new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)));

            await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(id), props);

            return Redirect(props.RedirectUri);
        }

        return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
    }

    protected virtual OpenIddictRequest ParseReturnUrl(string returnUrl)
    {
        if (returnUrl.IsNullOrWhiteSpace())
        {
            return null;
        }
        var parsedUrl = Uri.UnescapeDataString(returnUrl);
        var queryString = parsedUrl.Contains("?") ? parsedUrl.Split('?')[1] : null;
        if (queryString.IsNullOrWhiteSpace())
        {
            return null;
        }
        return new OpenIddictRequest(QueryHelpers.ParseQuery(queryString));
    }
}