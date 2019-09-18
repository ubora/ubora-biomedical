using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsProjectMemberGenericRequirementHandler<TRequirement> : ProjectAuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        public IsProjectMemberGenericRequirementHandler(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, object resource = null)
        {
            var isMember = Project.DoesSatisfy(new HasMember(context.User.GetId()));
            if (isMember)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}