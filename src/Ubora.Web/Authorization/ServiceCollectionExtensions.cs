﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Ubora.Web.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUboraAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, ProjectControllerRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsProjectMemberRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsProjectLeaderRequirement.Handler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(Policies.ProjectController), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new ProjectControllerRequirement());
                });

                options.AddPolicy(nameof(Policies.IsAuthenticatedUser), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                });

                options.AddPolicy(nameof(Policies.CanRemoveProjectMember), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
            });

            return services;
        }
    }
}
