using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web.Authorization.Requirements
{
    public class OrRequirement : IAuthorizationRequirement
    {
        public IAuthorizationRequirement[] Requirements { get; set; }

        public OrRequirement(params IAuthorizationRequirement[] requirements)
        {
            Requirements = requirements;
        }

        public class Handler : AuthorizationHandler<OrRequirement>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                OrRequirement requirement)
            {
                var authorizationService = _httpContextAccessor.HttpContext.RequestServices.GetService<IAuthorizationService>();

                var authorizationResults = new List<AuthorizationResult>();
                foreach (IAuthorizationRequirement authorizationRequirement in requirement.Requirements)
                {
                    authorizationResults.Add(await authorizationService.AuthorizeAsync(context.User, context.Resource, authorizationRequirement));
                }

                if (authorizationResults.Any(r => r.Succeeded))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
