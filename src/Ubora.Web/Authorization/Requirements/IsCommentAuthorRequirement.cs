using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsCommentAuthorRequirement : IAuthorizationRequirement 
    {
        public class Handler : AuthorizationHandler<IsCommentAuthorRequirement, Comment>
        {
            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                IsCommentAuthorRequirement requirement,
                Comment comment)
            {
                if (context.User.GetId() == comment.UserId)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
