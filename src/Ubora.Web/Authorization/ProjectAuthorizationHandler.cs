using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ubora.Web.Authorization
{
    public abstract class ProjectAuthorizationHandler<TRequirement>
        : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected ProjectAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            // Warning: Don't set HttpContext here -- always get latest instance from the accessor
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        // Scoped services need to use service location because all authorization-handlers are singletons.
        private IServiceProvider ServiceProvider => HttpContext.RequestServices;

        protected IQueryProcessor QueryProcessor => ServiceProvider.GetService<IQueryProcessor>();

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