using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Web._Features.ProjectCreation;
using Ubora.Web._Features.Projects.Dashboard;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Creation
{
    public class CreationControllerTests
    {
        [Fact(Skip = "Wait for TestControllerBase")]
        public void Create_Executes_Command()
        {
            var processorMock = new Mock<ICommandQueryProcessor>();

            var controller = new ProjectCreationController(processorMock.Object);

            var model = new CreateProjectViewModel
            {
                Title = "TestProjectName"
            };

            // Act
            var result = controller.Create(model);

            // Assert
            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(DashboardController.Dashboard));
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Dashboard");
        }
    }
}
