﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Data;

namespace Ubora.Web.Authorization
{
    public static class ServiceCollectionExtensions
    {
        /// <remarks> Policy-based authorization: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies </remarks>
        public static IServiceCollection AddUboraPolicyBasedAuthorization(this IServiceCollection services)
        {
            AddPolicies(services);
            AddRequirementHandlers(services);

            return services;
        }

        private static void AddRequirementHandlers(IServiceCollection services)
        {
            // NOTE: For logical OR evaluation implement multiple handlers for a requirement.
            services.AddSingleton<IAuthorizationHandler, ProjectControllerRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsUboraAdminGenericRequirementHandler<ProjectNonPublicContentViewingRequirement>>();
            services.AddSingleton<IAuthorizationHandler, IsProjectMemberGenericRequirementHandler<ProjectNonPublicContentViewingRequirement>>();
            services.AddSingleton<IAuthorizationHandler, IsProjectMemberRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsProjectLeaderRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsProjectMentorRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsWorkpackageOneNotLockedRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsEmailConfirmedRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsCommentAuthorRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, IsVoteNotGivenRequirement.Handler>();
            services.AddSingleton<IAuthorizationHandler, OrRequirement.Handler>();
        }

        private static void AddPolicies(IServiceCollection services)
        {
            // NOTE: All requirements of a policy have to succeed for successful authorization (AND evaluation).
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.IsAuthenticatedUser, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
                });

                options.AddPolicy(Policies.CanViewProjectNonPublicContent, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new ProjectNonPublicContentViewingRequirement());
                });

                options.AddPolicy(Policies.CanViewProjectHistory, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new OrRequirement(new IsProjectMemberRequirement(), new RolesAuthorizationRequirement(new string[] { ApplicationRole.Admin, ApplicationRole.ManagementGroup })));
                });

                options.AddPolicy(Policies.CanViewProjectRepository, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new OrRequirement(new IsProjectMemberRequirement(), new RolesAuthorizationRequirement(new string[] { ApplicationRole.Admin, ApplicationRole.ManagementGroup })));
                });

                options.AddPolicy(Policies.CanAddFileRepository, policyBuilder => 
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });

                options.AddPolicy(Policies.CanUpdateFileRepository, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });

                options.AddPolicy(Policies.CanWorkOnProjectContent, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new ProjectNonPublicContentViewingRequirement());
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });
                options.AddPolicy(Policies.ProjectController, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new ProjectControllerRequirement());
                });
                options.AddPolicy(Policies.CanRemoveProjectMember, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(Policies.CanRemoveProjectMentor, policyBuilder =>
                {
                    policyBuilder.RequireRole(ApplicationRole.Admin);
                });
                options.AddPolicy(Policies.CanReviewProjectWorkpackages, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMentorRequirement());
                });
                options.AddPolicy(Policies.CanSubmitWorkpackageForReview, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });

                options.AddPolicy(Policies.CanEditDesignPlanning, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });

                options.AddPolicy(Policies.CanEditWorkpackageOne, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                    policyBuilder.AddRequirements(new IsWorkpackageOneNotLockedRequirement());
                });
                options.AddPolicy(Policies.CanHideProjectFile, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(Policies.CanCreateProject, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsEmailConfirmedRequirement());
                });
                options.AddPolicy(Policies.CanJoinProject, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsEmailConfirmedRequirement());
                });
                options.AddPolicy(Policies.CanChangeProjectImage, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });
                options.AddPolicy(Policies.CanEditProjectTitleAndDescription, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });
                options.AddPolicy(Policies.CanDeleteProject, policyBuilder =>
                {
                    policyBuilder.RequireRole(ApplicationRole.Admin);
                });
                options.AddPolicy(Policies.CanAddProjectCandidate, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(Policies.CanEditProjectCandidate, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(Policies.CanChangeProjectCandidateImage, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(Policies.CanRemoveProjectCandidateImage, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(Policies.CanEditComment, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsCommentAuthorRequirement());
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });
                options.AddPolicy(Policies.CanVoteCandidate, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsVoteNotGivenRequirement());
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });
                options.AddPolicy(nameof(Policies.CanOpenWorkpackageThree), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectLeaderRequirement());
                });
                options.AddPolicy(nameof(Policies.CanInviteMentors), policyBuilder =>
                {
                    policyBuilder.RequireRole(ApplicationRole.Admin, ApplicationRole.ManagementGroup);
                });
                options.AddPolicy(nameof(Policies.CanEditAssignment), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new OrRequirement(new IsProjectLeaderRequirement(), new IsProjectMentorRequirement(), new RolesAuthorizationRequirement(new string[] { ApplicationRole.Admin })));
                });
                options.AddPolicy(Policies.CanPromoteMember, policyBuilder =>
                {
                    policyBuilder.RequireRole(ApplicationRole.ManagementGroup);
                });
            });
        }
    }
}
