using Microsoft.AspNetCore.Authorization;
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
            services.AddSingleton<IAuthorizationHandler, IsProjectMentorRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsWorkpackageOneNotLockedRequirement.Handler>();

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

                options.AddPolicy(nameof(Policies.CanReviewProjectWorkpackages), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectMentorRequirement());
                });

                options.AddPolicy(nameof(Policies.CanSubmitWorkpackageForReview), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanEditWorkpackageOne), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                    policyBuilder.AddRequirements(new IsWorkpackageOneNotLockedRequirement());

                });
            });

            return services;
        }
    }
}
