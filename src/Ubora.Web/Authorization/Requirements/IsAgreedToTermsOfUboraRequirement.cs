﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Ubora.Web.Authorization.Requirements
{
    /// <summary>
    /// Specifies whether the project has agreed with terms of UBORA in WP5.
    /// </summary>
    public class IsAgreedToTermsOfUboraRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsAgreedToTermsOfUboraRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
                AllowUnauthenticated = true;
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAgreedToTermsOfUboraRequirement requirement, object resource = null)
            {
                if (Project.IsAgreedToTermsOfUbora)
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
        }
    }
}
