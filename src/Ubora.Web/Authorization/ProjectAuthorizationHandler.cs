using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web.Authorization
{
    public abstract class ProjectAuthorizationHandler<TRequirement>
        : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        protected IQueryProcessor QueryProcessor;
        protected HttpContext HttpContext;

        protected ProjectAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IQueryProcessor queryProcessor)
        {
            QueryProcessor = queryProcessor;
            HttpContext = httpContextAccessor.HttpContext;
        }

        protected Project Project { get; private set; }

        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            Project = GetProject();

            if (Project == null)
            {
                // Don't handle requirements when project is not found.
                return;
            }

            await base.HandleAsync(context);
        }

        // Virtual for testing
        protected virtual Project GetProject()
        {
            var routeData = HttpContext.GetRouteData();
            var projectId = routeData.GetProjectId();

            return QueryProcessor.FindById<Project>(projectId);
        }
    }
}