using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects._Specifications;

namespace Ubora.Web.Authorization.Requirements
{
    /// <summary>
    /// Specifies whether the project has agreed with terms of UBORA in WP5.
    /// </summary>
    public class IsProjectAgreedToTermsOfUboraRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectAgreedToTermsOfUboraRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
                AllowUnauthenticated = true;
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectAgreedToTermsOfUboraRequirement requirement, object resource = null)
            {
                if (Project.DoesSatisfy(new IsAgreedToTermsOfUboraSpec()))
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
        }
    }
}
