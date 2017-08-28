using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>, IApplicationSignInManager
    {
        public ApplicationSignInManager(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger)
        {
        }
    }

    /// <summary>
    /// Helper interface to enable strict-mocking.
    /// </summary>
    public interface IApplicationSignInManager
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null);
        Task SignOutAsync();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task RefreshSignInAsync(ApplicationUser user);
        Task<ApplicationUser> GetTwoFactorAuthenticationUserAsync();
        Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient);
    }
}
