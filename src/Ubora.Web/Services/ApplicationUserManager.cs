using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public virtual Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            return base.FindByIdAsync(userId.ToString());
        }
    }

    /// <summary>
    /// Helper interface to enable strict-mocking.
    /// </summary>
    public interface IApplicationUserManager
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal);
        string GetUserId(ClaimsPrincipal principal);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<string> GetUserIdAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo login);     
        Task<string> GetEmailAsync(ApplicationUser user);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(ApplicationUser user);
        Task<string> GenerateTwoFactorTokenAsync(ApplicationUser user, string tokenProvider);
    }
}