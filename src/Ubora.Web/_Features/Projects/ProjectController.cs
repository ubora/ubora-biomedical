using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects
{
    [Route("projects/{projectId:Guid}")]
    [Route("projects/{projectId:Guid}/[controller]/[action]/{id?}")]
    public abstract class ProjectController : UboraController
    {
        private readonly ICommandQueryProcessor _processor;

        private Project _project;
        public Project Project => _project ?? (_project = _processor.FindById<Project>(ProjectId));

        public Guid ProjectId => Guid.Parse((string)RouteData.Values["projectId"]);

        protected ProjectController(ICommandQueryProcessor processor) : base(processor)
        {
            _processor = processor;
        }

        protected virtual UserInfo UserInfo => User.GetInfo();

        public static class Policies
        {
            public const string FromProject = "AtleastMember";
        }
    }
}