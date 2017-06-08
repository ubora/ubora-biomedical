using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Projects
{
    [Route("Projects/{projectId:Guid}")]
    [Route("Projects/{projectId:Guid}/[action]/{id?}")]
    [Route("Projects/{projectId:Guid}/[controller]/[action]/{id?}")]
    [Authorize(Policy = nameof(Policies.ProjectController))]
    public abstract class ProjectController : UboraController
    {
        private readonly ICommandQueryProcessor _processor;

        protected ProjectController(ICommandQueryProcessor processor) : base(processor)
        {
            _processor = processor;
        }

        protected Guid ProjectId => RouteData.GetProjectId();

        private Project _project;
        protected Project Project => _project ?? (_project = _processor.FindById<Project>(ProjectId));

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
        public class DisableProjectControllerAuthorizationAttribute : Attribute, IDisablesProjectControllerAuthorizationFilter
        {
        }
    }
}