using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features;
using Ubora.Web._Features.Projects;
using Xunit;

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

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
            };

            AssertHasAuthorizeAttributes(typeof(ProjectController), methodPolicies);
        }
    }
}