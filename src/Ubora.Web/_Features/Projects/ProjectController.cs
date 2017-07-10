using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Projects
{
    public class ProjectRouteAttribute : RouteAttribute
    {
        public ProjectRouteAttribute(string template) : base("Projects/{projectId:Guid}/" + template)
        {
        }
    }

    [ProjectRoute("[controller]/[action]")]
    [Authorize(Policy = nameof(Policies.ProjectController))]
    public abstract class ProjectController : UboraController
    {
        protected Guid ProjectId => RouteData.GetProjectId();

        private Project _project;
        protected Project Project => _project ?? (_project = QueryProcessor.FindById<Project>(ProjectId));

        protected void ExecuteUserProjectCommand<T>(T command) where T : UserProjectCommand
        {
            command.ProjectId = ProjectId;
            base.ExecuteUserCommand(command);
        }

        [Obsolete]
        protected new void ExecuteUserCommand<T>(T command) where T : IUserCommand
        {
            throw new NotSupportedException($"Use {nameof(ExecuteUserProjectCommand)} instead.");
        }

        /// <summary>
        /// Disables <see cref="ProjectControllerRequirement.Handler"/>.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        protected class DisableProjectControllerAuthorizationAttribute : Attribute, IDisablesProjectControllerAuthorizationFilter
        {
        }
    }
}