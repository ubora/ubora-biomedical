using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Services;
using Ubora.Domain.Discussions;

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
                if (context.User.Identity.IsAuthenticated && context.User.GetId() == comment.UserId)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
