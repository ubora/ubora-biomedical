using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Routing;
using Ubora.Web._Features;

namespace Ubora.Web.Tests._Features.Projects
{
    public abstract class ProjectControllerTestsBase : UboraControllerTestsBase
    {
        protected Guid ProjectId { get; }

        protected ProjectControllerTestsBase() : base()
        {
            ProjectId = Guid.NewGuid();
        }

        protected override void SetUpForTest(UboraController controller)
        {
            base.SetUpForTest(controller);

            if (controller.ControllerContext.RouteData == null)
            {
                controller.ControllerContext.RouteData = new RouteData();
            }

            controller.RouteData.Values
                .Add("projectId", ProjectId.ToString());
        }
    }
}