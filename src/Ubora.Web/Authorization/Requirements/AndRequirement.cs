using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web.Authorization.Requirements
{
    public class AndRequirement : IAuthorizationRequirement
    {
        public IAuthorizationRequirement[] Requirements { get; set; }

        public AndRequirement(params IAuthorizationRequirement[] requirements)
        {
            Requirements = requirements;
        }

        public class Handler : AuthorizationHandler<AndRequirement>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                AndRequirement andRequirement)
            {
                var authorizationService = _httpContextAccessor.HttpContext.RequestServices.GetService<IAuthorizationService>();

                var authorizationResults = new List<AuthorizationResult>();
                foreach (IAuthorizationRequirement innerRequirement in andRequirement.Requirements)
                {
                    authorizationResults.Add(await authorizationService.AuthorizeAsync(context.User, context.Resource, innerRequirement));
                }

                if (authorizationResults.All(r => r.Succeeded))
                {
                    context.Succeed(andRequirement);
                }

                if (authorizationResults.Any(r => r.Failure?.FailCalled == true))
                {
                    context.Fail();
                }
            }
        }
    }
}