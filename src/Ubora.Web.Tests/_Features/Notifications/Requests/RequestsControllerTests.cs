using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications.Join;
using Ubora.Web._Features.Notifications;
using Ubora.Web._Features.Notifications.Requests;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications.Requests
{
    public class RequestsControllerTests : UboraControllerTestsBase
    {
        private RequestsController _requestsController;
        private Mock<ICommandQueryProcessor> _commandQueryProcessorMock;

        public RequestsControllerTests()
        {
            _commandQueryProcessorMock = new Mock<ICommandQueryProcessor>();
            _requestsController = new RequestsController(_commandQueryProcessorMock.Object);
            SetUserContext(_requestsController);
        }

        [Fact]
        public void Accept_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            var vm = new RequestPartialViewModel { RequestId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<AcceptRequestToJoinProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_requestsController.Accept(vm);

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Accept_Returns_BadResult_If_Command_Fails()
        {
            var vm = new RequestPartialViewModel { RequestId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<AcceptRequestToJoinProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = _requestsController.Accept(vm);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Decline_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            var vm = new RequestPartialViewModel { RequestId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<DeclineRequestToJoinProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_requestsController.Decline(vm);

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Decline_Returns_BadResult_If_Command_Fails()
        {
            var vm = new RequestPartialViewModel { RequestId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<DeclineRequestToJoinProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = _requestsController.Decline(vm);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
