using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web.Authorization
{
    public class OverrideProjectPolicyAttribute : TypeFilterAttribute, IOverrideProjectPolicy
    {
        public OverrideProjectPolicyAttribute(string newPolicy) : base(typeof(AuthorizePolicyFilter))
        {
            Arguments = new object[] { newPolicy };
        }

        private class AuthorizePolicyFilter : IActionFilter
        {
            private string Policy { get; }

            private readonly IAuthorizationService _authorizationService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public AuthorizePolicyFilter(string policy, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
            {
                Policy = policy;
                _authorizationService = authorizationService;
                _httpContextAccessor = httpContextAccessor;
            }

            public async void OnActionExecuting(ActionExecutingContext context)
            {
                var user = _httpContextAccessor.HttpContext.User;

                var isNotAuthorized = !await _authorizationService.AuthorizeAsync(user, null, Policy);
                if (isNotAuthorized)
                {
                    // TODO(Kaspar Kallas): Authentication schemes?
                    context.Result = new ChallengeResult();
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}