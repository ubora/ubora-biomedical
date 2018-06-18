using System;
using System.Threading.Tasks;
using FluentAssertions;
using Marten.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Resources;
using Ubora.Web._Features.Resources.Models;
using Xunit;

namespace Ubora.Web.Tests._Features.Resources
{
    public class ResourcesControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<ResourcesController> _controllerMock;

        public ResourcesController ControllerUnderTest => _controllerMock.Object;

        public ResourcesControllerTests()
        {
            var eventStoreMock = new Mock<IEventStore>();
            _controllerMock = new Mock<ResourcesController>(eventStoreMock.Object)
            {
                CallBase = true
            };
            SetUpForTest(ControllerUnderTest);
        }

        [Fact]
        public async Task Add_HttpPost_Returns_UnauthorizedResult_When_User_Is_Not_Authorized()
        {
            // Act
            var result = await ControllerUnderTest.Add(new AddResourcePostModel());
            
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
                .Setup(a => a.AuthorizeAsync(ControllerUnderTest.User, null, Policies.CanAddResourcePage))
                .ReturnsAsync(AuthorizationResult.Success);
            
            var postModel = new AddResourcePostModel();

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
                .Setup(a => a.AuthorizeAsync(ControllerUnderTest.User, null, Policies.CanAddResourcePage))
                .ReturnsAsync(AuthorizationResult.Success);
            
            var postModel = new AddResourcePostModel();

            // Act
            var result = await ControllerUnderTest.Add(postModel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Add_HttpPost_Redirects_To_ResourcePage_After_Successful_Command_Execution()
        {
            var postModel = new AddResourcePostModel
            {
                Body = "testBody",
                Title = "testTitle"
            };

            CreateResourcePageCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<CreateResourcePageCommand>()))
                .Callback<CreateResourcePageCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var resourcePage = new ResourcePage().Set(x => x.Slug, Slug.Generate("test slug"));

            QueryProcessorMock
                .Setup(q => q.FindById<ResourcePage>(It.IsAny<Guid>()))
                .Returns(resourcePage);
            
            AuthorizationServiceMock
                .Setup(a => a.AuthorizeAsync(ControllerUnderTest.User, null, Policies.CanAddResourcePage))
                .ReturnsAsync(AuthorizationResult.Success);

            // Act
            var result = (RedirectToActionResult) await ControllerUnderTest.Add(postModel);

            // Assert
            result.ActionName.Should().Be(nameof(ResourcesController.Read));
            result.RouteValues["slug"] = "test-slug";

            executedCommand.ResourceId.Should().NotBe(default(Guid));
            executedCommand.Content.Body.Value.Should().Be("testBody");
            executedCommand.Content.Title.Should().Be("testTitle");

            QueryProcessorMock.Verify(q => q.FindById<ResourcePage>(executedCommand.ResourceId));
        }
    }
}
