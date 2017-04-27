using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web.Areas.Projects.Controllers;
using Ubora.Web.Areas.Projects.Views.Create;
using Xunit;

namespace Ubora.Web.Tests.Areas.Projects
{
    public class CreateControllerTests
    {
        [Fact]
        public void Create_Executes_Command()
        {
            var commandProcessorMock = new Mock<ICommandProcessor>();

            var controller = new CreateController(commandProcessorMock.Object, Mock.Of<IMapper>());

            var model = new CreatePostModel
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
