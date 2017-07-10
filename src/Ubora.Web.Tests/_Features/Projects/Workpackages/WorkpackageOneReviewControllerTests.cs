﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Projects.Workpackages.Reviews;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageOneReviewControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageOneReviewController _workpackageOneReviewController;

        public WorkpackageOneReviewControllerTests()
        {
            _workpackageOneReviewController = new WorkpackageOneReviewController()
            {
                Url = Mock.Of<IUrlHelper>()
            };
            SetMocks(_workpackageOneReviewController);
            SetProjectAndUserContext(_workpackageOneReviewController);

            var dummyWorkpackage = Mock.Of<WorkpackageOne>(x => x.Reviews == new List<WorkpackageReview>());
            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(dummyWorkpackage);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Submit_Button_Is_Hidden_Completely_When_Workpackage_Is_Under_Review_Or_Has_Been_Accepted(
            bool isReviewInProcess,
            bool hasBeenAccepted)
        {
            var workpackage = Mock.Of<WorkpackageOne>(
                x => x.HasReviewInProcess == isReviewInProcess 
                && x.HasBeenAccepted == hasBeenAccepted 
                && x.Reviews == new List<WorkpackageReview>());

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackage);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsHiddenCompletely
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Is_Hidden_With_Message_When_User_Is_Not_Authorized()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(false);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsHiddenWithMessage
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Can_Be_Visible()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(true);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel) result.Model;
            viewModel
                .SubmitForReviewButton.IsVisible
                .Should().BeTrue();
        }
    }
}
