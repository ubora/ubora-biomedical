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
    public class WorkpackageOneReviewControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageOneReviewController _workpackageOneReviewController;

        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;

        public WorkpackageOneReviewControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();

            _workpackageOneReviewController = new WorkpackageOneReviewController(_processorMock.Object, _mapperMock.Object, _authorizationServiceMock.Object)
            {
                Url = Mock.Of<IUrlHelper>()
            };
            SetProjectAndUserContext(_workpackageOneReviewController);
            var dummyWorkpackage = Mock.Of<WorkpackageOne>(x => x.Reviews == new List<WorkpackageReview>());
            _workpackageOneReviewController.Set(x => x.WorkpackageOne, dummyWorkpackage);
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

            _workpackageOneReviewController.Set(x => x.WorkpackageOne, workpackage);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            result.Model.As<WorkpackageReviewListViewModel>()
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
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            result.Model.As<WorkpackageReviewListViewModel>()
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
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            result.Model.As<WorkpackageReviewListViewModel>()
                .SubmitForReviewButton.IsVisible
                .Should().BeTrue();
        }
    }
}
