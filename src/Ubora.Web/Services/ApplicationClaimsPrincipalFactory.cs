using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Models;

namespace Ubora.Web.Services
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly IQueryProcessor _queryProcessor;

        public ApplicationClaimsPrincipalFactory(IQueryProcessor queryProcessor, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var claimsPrincipal = await base.CreateAsync(user);
            var claimsIdentity = (ClaimsIdentity) claimsPrincipal.Identity;

            var userProfile = _queryProcessor.FindById<UserProfile>(user.Id);
            claimsIdentity.AddClaim(new Claim(ApplicationUser.FullNameClaimType, userProfile.FullName));

            return claimsPrincipal;
        }
    }
}