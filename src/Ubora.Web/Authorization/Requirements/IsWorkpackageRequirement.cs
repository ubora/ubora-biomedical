using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsWorkpackageRequirement : IAuthorizationRequirement
    {
        public WorkpackageType WorkpackageType { get; set; }
        
        public IsWorkpackageRequirement(WorkpackageType workpackageType)
        {
            WorkpackageType = workpackageType;
        }
        
        public class Handler : ProjectAuthorizationHandler<IsWorkpackageRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                IsWorkpackageRequirement requirement)
            {    
                switch (requirement.WorkpackageType)
                {
                    case WorkpackageType.Three:
                        var workpackageThree = QueryProcessor.FindById<WorkpackageThree>(Project.Id);
                        if (workpackageThree != null)
                        {
                            context.Succeed(requirement);
                        }
                        break;
                    
                    case WorkpackageType.Four:
                        var workpackageFour = QueryProcessor.FindById<WorkpackageFour>(Project.Id);
                        if (workpackageFour != null)
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