using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects
{
    [Route("Projects/{projectId:Guid}")]
    [Route("Projects/{projectId:Guid}/[action]/{id?}")]
    [Route("Projects/{projectId:Guid}/[controller]/[action]/{id?}")]
    [Authorize(Policy = nameof(Policies.IsProjectMember))]
    public abstract class ProjectController : UboraController
    {
        protected IQueryProcessor QueryProcessor { get; private set; }

        private readonly ICommandQueryProcessor _processor;

        protected ProjectController(ICommandQueryProcessor processor) : base(processor)
        {
            _processor = processor;
            QueryProcessor = _processor;
        }

        protected Guid ProjectId => Guid.Parse((string) RouteData.Values["projectId"]);

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
        /// Disables <see cref="IsProjectMemberAuthorizationHandler"/>.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        protected class DisableProjectControllerAuthorizationPolicyAttribute : Attribute, IDisablesProjectAuthorizationPolicyFilter
        {
        }
    }
}