using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Ubora.Web.Authorization
{
    public abstract class ProjectAuthorizationHandler<TRequirement, TResource>
        : ProjectAuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        protected ProjectAuthorizationHandler(IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, object resource = null)
        {
            return HandleRequirementAsync(context, requirement, (TResource)resource);
        }

        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource);
    }

    public abstract class ProjectAuthorizationHandler<TRequirement>
        : IAuthorizationHandler
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

        protected virtual IQueryProcessor QueryProcessor => ServiceProvider.GetService<IQueryProcessor>();

        protected Project Project { get; private set; }
        public bool AllowUnauthenticated { get; protected set; }

        protected virtual Project GetProject()
        {
            var routeData = HttpContext.GetRouteData();
            var projectId = routeData.GetProjectId();

            return QueryProcessor.FindById<Project>(projectId);
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            Project = GetProject();

            if (Project == null)
            {
                // Don't handle requirements when project is not found.
                return;
            }

            if (!AllowUnauthenticated && !context.User.Identity.IsAuthenticated)
            {
                return;
            }

            foreach (var req in context.Requirements.OfType<TRequirement>())
            {
                await HandleRequirementAsync(context, req, context.Resource);
            }
        }

        protected virtual bool IsAuthenticationSuitable(AuthorizationHandlerContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }

        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, object resource = null);
    }
}