using System.Security.Claims;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task<bool> IsAuthorizedAsync(this IAuthorizationService authorizationService, ClaimsPrincipal user, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(user, resource: null, policyName: policyName)).Succeeded;
        }

        public static async Task<bool> IsAuthorizedAsync(this IAuthorizationService authorizationService, ClaimsPrincipal user, object resource, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(user, resource: resource, policyName: policyName)).Succeeded;
        }
    }
}
