using System;
using Ubora.Web._Features.Projects;

namespace Ubora.Web.Tests._Features.Projects
{
    public abstract class ProjectControllerTestsBase : UboraControllerTestsBase
    {
        protected Guid ProjectId { get; }

        protected ProjectControllerTestsBase() : base()
        {
            ProjectId = Guid.NewGuid();
        }

        protected void SetProjectAndUserContext(ProjectController controller)
        {
            SetUserContext(controller);

            controller.RouteData.Values
                .Add("projectId", ProjectId.ToString());
        }
    }
}