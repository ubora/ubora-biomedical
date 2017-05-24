using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects
{
    public abstract class ProjectViewComponent : ViewComponent
    {
        protected IQueryProcessor QueryProcessor { get; private set; }

        private readonly ICommandQueryProcessor _processor;

        protected ProjectViewComponent(ICommandQueryProcessor processor)
        {
            _processor = processor;
            QueryProcessor = _processor;
        }

        public Guid ProjectId => Guid.Parse((string)RouteData.Values["projectId"]);

        private Project _project;
        public Project Project => _project ?? (_project = _processor.FindById<Project>(ProjectId));
    }
}