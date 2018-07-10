using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ResourcesArea.ResourcePageCreation;
using Ubora.Web._Areas.ResourcesArea.ResourcePageCreation.Models;
using Xunit;

namespace Ubora.Web.Tests._Areas.ResourcesArea.ResourcePageCreation
{
    public class ResourcePageCreationControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<ResourcePageCreationController> _controllerMock;

        public ResourcePageCreationController ControllerUnderTest => _controllerMock.Object;

        public ResourcePageCreationControllerTests()
        {
            _controllerMock = new Mock<ResourcePageCreationController>()
            {
                CallBase = true
            };
            SetUpForTest(ControllerUnderTest);
        }

        [Fact]
        public async Task Add_HttpPost_Returns_UnauthorizedResult_When_User_Is_Not_Authorized()
        {
            // Act
            var result = await ControllerUnderTest.Add(new CreateResourcePagePostModel());

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Add_HttpPost_Returns_Same_View_When_ModelState_Is_Not_Valid()
        {
            ControllerUnderTest.ModelState.AddModelError("", "dummyError");

            var expectedResult = new ViewResult();
            _controllerMock
                .Setup(c => c.Add()).Returns(expectedResult);

            AuthorizationServiceMock
                .Setup(a => a.AuthorizeAsync(ControllerUnderTest.User, null, Policies.CanManageResourcePages))
                .ReturnsAsync(AuthorizationResult.Success);

            var postModel = new CreateResourcePagePostModel();

            // Act
            var result = await ControllerUnderTest.Add(postModel);

            // Assert
            result.Should().Be(expectedResult);
            CommandProcessorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Add_HttpPost_Returns_Same_View_When_Command_Execution_Is_Unsuccessful()
        {
            var expectedResult = new ViewResult();
            _controllerMock
                .Setup(c => c.Add()).Returns(expectedResult);

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<CreateResourcePageCommand>()))
                .Returns(CommandResult.Failed("dummyError"));

            AuthorizationServiceMock
                .Setup(a => a.AuthorizeAsync(ControllerUnderTest.User, null, Policies.CanManageResourcePages))
                .ReturnsAsync(AuthorizationResult.Success);

            var postModel = new CreateResourcePagePostModel();

            // Act
            var result = await ControllerUnderTest.Add(postModel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Add_HttpPost_Redirects_To_ResourcePage_After_Successful_Command_Execution()
        {
            var postModel = new CreateResourcePagePostModel
            {
                Body = "testBody",
                Title = "testTitle"
            };

            CreateResourcePageCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<CreateResourcePageCommand>()))
                .Callback<CreateResourcePageCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            AuthorizationServiceMock
                .Setup(a => a.AuthorizeAsync(ControllerUnderTest.User, null, Policies.CanManageResourcePages))
                .ReturnsAsync(AuthorizationResult.Success);

            // Act
            var result = (RedirectToActionResult)await ControllerUnderTest.Add(postModel);

            // Assert
            result.ActionName.Should().Be(nameof(ResourcePagesController.Read));
            result.ControllerName.Should().Be(nameof(ResourcePagesController).RemoveSuffix());

            executedCommand.ResourcePageId.Should().NotBe(default(Guid));
            executedCommand.Body.Value.Should().Be("testBody");
            executedCommand.Title.Should().Be("testTitle");
        }
    }
}
