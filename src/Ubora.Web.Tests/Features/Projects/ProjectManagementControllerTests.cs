using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Features.Projects;
using Xunit;
using FluentAssertions;
using Ubora.Web.Features.ProjectManagement;

namespace Ubora.Web.Tests.Features.Projects
{
    public class ProjectManagementControllerTests
    {
        [Fact(Skip = "Wait for TestControllerBase")]
        public void Create_Executes_Command()
        {
            var processorMock = new Mock<ICommandQueryProcessor>();

            var controller = new ProjectsController(processorMock.Object, Mock.Of<IMapper>());

            var model = new CreateViewModel
            {
                Title = "TestProjectName"
            };

            // Act
            var result = controller.Create(model);

            // Assert
            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(ProjectManagementController.Index));
            result.As<RedirectToActionResult>().ControllerName.Should().Be("ProjectManagement");
        }
    }
}
