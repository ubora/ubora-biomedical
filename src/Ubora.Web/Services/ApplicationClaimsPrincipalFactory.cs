using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web.Services
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IStorageProvider _storageProvider;

        public ApplicationClaimsPrincipalFactory(IQueryProcessor queryProcessor, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IStorageProvider storageProvider) : base(userManager, roleManager, optionsAccessor)
        {
            _queryProcessor = queryProcessor;
            _storageProvider = storageProvider;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var claimsPrincipal = await base.CreateAsync(user);
            var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

            var userProfile = _queryProcessor.FindById<UserProfile>(user.Id);
            claimsIdentity.AddClaim(new Claim(ApplicationUser.FullNameClaimType, userProfile.FullName));

            var profilePictureUrl = _storageProvider.GetDefaultOrBlobUrl(userProfile);
            claimsIdentity.AddClaim(new Claim(ApplicationUser.ProfilePictureUrlClaimType, profilePictureUrl));

            var userEmail = userProfile.Email;
            claimsIdentity.AddClaim(new Claim(ApplicationUser.EmailClaimType, userEmail));

            return claimsPrincipal;
        }
    }
}