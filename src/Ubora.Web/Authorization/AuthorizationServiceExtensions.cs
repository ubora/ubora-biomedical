using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task<bool> IsAuthorized(this IAuthorizationService authorizationService, ClaimsPrincipal user, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(user, resource: null, policyName: policyName)).Succeeded;
        }
    }
}
