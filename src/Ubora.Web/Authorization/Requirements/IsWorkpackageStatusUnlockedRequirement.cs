using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsWorkpackageStatusUnlockedRequirement : IAuthorizationRequirement
    {
        public WorkpackageType WorkpackageType { get; set; }
        
        public IsWorkpackageStatusUnlockedRequirement(WorkpackageType workpackageType)
        {
            WorkpackageType = workpackageType;
        }
        
        public class Handler : ProjectAuthorizationHandler<IsWorkpackageStatusUnlockedRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                IsWorkpackageStatusUnlockedRequirement requirement)
            {
                var result = QueryProcessor.ExecuteQuery(new GetStatusesOfProjectWorkpackagesQuery(Project.Id));

                switch (requirement.WorkpackageType)
                {
                    case WorkpackageType.Three:
                        if (result.Wp3Status == WorkpackageStatus.UnLocked)
                        {
                            context.Succeed(requirement);
                        }
                        break;
                    
                    case WorkpackageType.Four:
                        if (result.Wp4Status == WorkpackageStatus.UnLocked)
                        {
                            context.Succeed(requirement);
                        }
                        break;
                }
                
                return Task.CompletedTask;
            }
        }
    }
}