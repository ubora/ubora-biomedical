using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects
{
    [Authorize(Policy = nameof(ProjectController))]
    [Route("Projects/{projectId:Guid}")]
    [Route("Projects/{projectId:Guid}/[controller]/[action]/{id?}")]
    public abstract class ProjectController : UboraController
    {
        private readonly ICommandQueryProcessor _processor;

        protected ProjectController(ICommandQueryProcessor processor) : base(processor)
        {
            _processor = processor;
        }

        public Guid ProjectId => Guid.Parse((string) RouteData.Values["projectId"]);

        private Project _project;
        public Project Project => _project ?? (_project = _processor.FindById<Project>(ProjectId));

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
    }
}