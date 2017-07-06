using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web.Authorization;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features.Projects.Workpackages.Reviews;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageTwoReviewControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageTwoReviewController _workpackageTwoReviewController;

        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;

        public WorkpackageTwoReviewControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();

            _workpackageTwoReviewController = new WorkpackageTwoReviewController(_processorMock.Object, _mapperMock.Object, _authorizationServiceMock.Object)
            {
                Url = Mock.Of<IUrlHelper>()
            };
            SetProjectAndUserContext(_workpackageTwoReviewController);
            var dummyWorkpackage = Mock.Of<WorkpackageTwo>(x => x.Reviews == new List<WorkpackageReview>());
            _workpackageTwoReviewController.Set(x => x.WorkpackageTwo, dummyWorkpackage);
        }
        
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Submit_Button_Is_Hidden_Completely_When_Workpackage_Is_Under_Review_Or_Has_Been_Accepted(
            bool isReviewInProcess,
            bool hasBeenAccepted)
        {
            var workpackage = Mock.Of<WorkpackageTwo>(
                x => x.HasReviewInProcess == isReviewInProcess 
                && x.HasBeenAccepted == hasBeenAccepted 
                && x.Reviews == new List<WorkpackageReview>());

            _workpackageTwoReviewController.Set(x => x.WorkpackageTwo, workpackage);

            // Act
            var result = (ViewResult)await _workpackageTwoReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsHiddenCompletely
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Is_Hidden_With_Message_When_User_Is_Not_Authorized()
        {
            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(false);

            // Act
            var result = (ViewResult)await _workpackageTwoReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsHiddenWithMessage
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Can_Be_Visible()
        {
            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(true);

            // Act
            var result = (ViewResult)await _workpackageTwoReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsVisible
                .Should().BeTrue();
        }
    }
}
