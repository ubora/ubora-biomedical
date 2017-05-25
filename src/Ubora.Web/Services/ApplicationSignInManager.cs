using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor accessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsPrincipalFactory, IOptions<IdentityOptions> options, ILogger<SignInManager<ApplicationUser>> logger) : base(userManager, accessor, claimsPrincipalFactory, options, logger)
        {
        }
    }
}
