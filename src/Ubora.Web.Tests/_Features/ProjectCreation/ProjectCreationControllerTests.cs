using System.Collections.Generic;
using Ubora.Web._Features.ProjectCreation;
using Ubora.Web.Authorization;
using Ubora.Web.Tests.Helper;
using Xunit;

namespace Ubora.Web.Tests._Features.ProjectCreation
{
    public class ProjectCreationControllerTests : UboraControllerTestsBase
    {
        private readonly ProjectCreationController _controller;
        public ProjectCreationControllerTests()
        {
            _controller = new ProjectCreationController();
            SetUpForTest(_controller);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var rolesAndPoliciesAuthorizations = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(ProjectCreationController.Create),
                    Policies = new []{ nameof(Policies.CanCreateProject) }
                }
            };

            AssertHasAuthorizeAttributes(typeof(ProjectCreationController), rolesAndPoliciesAuthorizations);
        }
    }
}
