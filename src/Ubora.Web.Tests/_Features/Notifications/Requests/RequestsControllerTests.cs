﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
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

        public RequestsControllerTests()
        {
            _requestsController = new RequestsController();
            SetMocks(_requestsController);
            SetUserContext(_requestsController);
        }

        [Fact]
        public void Accept_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            var model = new RequestPartialViewModel { RequestId = Guid.NewGuid() };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<AcceptRequestToJoinProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_requestsController.Accept(model);

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Accept_Returns_ModelState_With_Error_If_Command_Fails()
        {
            var model = new RequestPartialViewModel { RequestId = Guid.NewGuid() };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<AcceptRequestToJoinProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (RedirectToActionResult)_requestsController.Accept(model);

            // Assert
            _requestsController.ModelState.ErrorCount.Should().Be(1);
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Decline_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            var model = new RequestPartialViewModel { RequestId = Guid.NewGuid() };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<DeclineRequestToJoinProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_requestsController.Decline(model);

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Decline_Returns_ModelState_With_Error_If_Command_Fails()
        {
            var model = new RequestPartialViewModel { RequestId = Guid.NewGuid() };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<DeclineRequestToJoinProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (RedirectToActionResult)_requestsController.Decline(model);

            // Assert
            _requestsController.ModelState.ErrorCount.Should().Be(1);
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }
    }
}
