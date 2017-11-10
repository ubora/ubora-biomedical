using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Web.Data;

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
            services.AddSingleton<IAuthorizationHandler, IsEmailConfirmedRequirement.Handler>();

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
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanReviewProjectWorkpackages), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMentorRequirement());
                });

                options.AddPolicy(nameof(Policies.CanSubmitWorkpackageForReview), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanEditWorkpackageOne), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                    policyBuilder.AddRequirements(new IsWorkpackageOneNotLockedRequirement());
                });

                options.AddPolicy(nameof(Policies.CanHideProjectFile), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanCreateProject), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsEmailConfirmedRequirement());
                });

                options.AddPolicy(nameof(Policies.CanJoinProject), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsEmailConfirmedRequirement());
                });

                options.AddPolicy(nameof(Policies.CanChangeProjectImage), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });

                options.AddPolicy(nameof(Policies.CanEditProjectDescription), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });

                options.AddPolicy(nameof(Policies.CanDeleteProject), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.RequireRole(ApplicationRole.Admin);
                });

                options.AddPolicy(nameof(Policies.CanAddProjectCandidate), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanEditProjectCandidate), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanChangeProjectCandidateImage), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(nameof(Policies.CanRemoveProjectCandidateImage), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
            });

            return services;
        }
    }
}
