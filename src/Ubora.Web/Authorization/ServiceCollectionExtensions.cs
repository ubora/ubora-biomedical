using Microsoft.Extensions.DependencyInjection;

namespace Ubora.Web.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUboraAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(Policies.IsProjectMember), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsProjectMemberRequirement());
                });

                options.AddPolicy(nameof(Policies.IsAuthenticatedUser), policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsAuthenticatedUserRequirement());
                });
            });

            return services;
        }
    }
}
