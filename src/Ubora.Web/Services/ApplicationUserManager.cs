using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
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
    }

    /// <summary>
    /// Helper interface to enable strict-mocking.
    /// </summary>
    public interface IApplicationUserManager
    {
        string GetUserName(ClaimsPrincipal principal);
        string GetUserId(ClaimsPrincipal principal);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal);
        Task<string> GenerateConcurrencyStampAsync(ApplicationUser user);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<IdentityResult> DeleteAsync(ApplicationUser user);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        string NormalizeKey(string key);
        Task UpdateNormalizedUserNameAsync(ApplicationUser user);
        Task<string> GetUserNameAsync(ApplicationUser user);
        Task<IdentityResult> SetUserNameAsync(ApplicationUser user, string userName);
        Task<string> GetUserIdAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> HasPasswordAsync(ApplicationUser user);
        Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<IdentityResult> RemovePasswordAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task<string> GetSecurityStampAsync(ApplicationUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(ApplicationUser user);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IdentityResult> RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey);
        Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo login);
        Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user);
        Task<IdentityResult> AddClaimAsync(ApplicationUser user, Claim claim);
        Task<IdentityResult> AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);
        Task<IdentityResult> ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim);
        Task<IdentityResult> RemoveClaimAsync(ApplicationUser user, Claim claim);
        Task<IdentityResult> RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
        Task<string> GetEmailAsync(ApplicationUser user);
        Task<IdentityResult> SetEmailAsync(ApplicationUser user, string email);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task UpdateNormalizedEmailAsync(ApplicationUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<string> GenerateChangeEmailTokenAsync(ApplicationUser user, string newEmail);
        Task<IdentityResult> ChangeEmailAsync(ApplicationUser user, string newEmail, string token);
        Task<string> GetPhoneNumberAsync(ApplicationUser user);
        Task<IdentityResult> SetPhoneNumberAsync(ApplicationUser user, string phoneNumber);
        Task<IdentityResult> ChangePhoneNumberAsync(ApplicationUser user, string phoneNumber, string token);
        Task<bool> IsPhoneNumberConfirmedAsync(ApplicationUser user);
        Task<string> GenerateChangePhoneNumberTokenAsync(ApplicationUser user, string phoneNumber);
        Task<bool> VerifyChangePhoneNumberTokenAsync(ApplicationUser user, string token, string phoneNumber);
        Task<bool> VerifyUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose, string token);
        Task<string> GenerateUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose);
        void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<ApplicationUser> provider);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(ApplicationUser user);
        Task<bool> VerifyTwoFactorTokenAsync(ApplicationUser user, string tokenProvider, string token);
        Task<string> GenerateTwoFactorTokenAsync(ApplicationUser user, string tokenProvider);
        Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user);
        Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled);
        Task<bool> IsLockedOutAsync(ApplicationUser user);
        Task<IdentityResult> SetLockoutEnabledAsync(ApplicationUser user, bool enabled);
        Task<bool> GetLockoutEnabledAsync(ApplicationUser user);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user);
        Task<IdentityResult> SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd);
        Task<IdentityResult> AccessFailedAsync(ApplicationUser user);
        Task<IdentityResult> ResetAccessFailedCountAsync(ApplicationUser user);
        Task<int> GetAccessFailedCountAsync(ApplicationUser user);
        Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim);
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        Task<string> GetAuthenticationTokenAsync(ApplicationUser user, string loginProvider, string tokenName);
        Task<IdentityResult> SetAuthenticationTokenAsync(ApplicationUser user, string loginProvider, string tokenName, string tokenValue);
        Task<IdentityResult> RemoveAuthenticationTokenAsync(ApplicationUser user, string loginProvider, string tokenName);
    }
}