using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Projects
{
    public abstract class ProjectViewComponent : ViewComponent
    {
        protected IQueryProcessor QueryProcessor { get; private set; }

        protected ProjectViewComponent(IQueryProcessor processor)
        {
            QueryProcessor = processor;
        }

        protected Guid ProjectId => RouteData.GetProjectId();

        private Project _project;
        protected Project Project => _project ?? (_project = QueryProcessor.FindById<Project>(ProjectId));
    }
}