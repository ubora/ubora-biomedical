using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageFiveControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageFiveController _controller;

        public WorkpackageFiveControllerTests()
        {
            _controller = new WorkpackageFiveController();

            SetUpForTest(_controller);

            QueryProcessorMock.Setup(q => q.FindById<WorkpackageFive>(this.ProjectId)).Returns(WorkpackageFive);
        }

        private WorkpackageFive WorkpackageFive { get; set; } = new WorkpackageFive();

        [Fact]
        public void AgreeToTermsOfUbora_Post_Can_Return_Unauthorized()
        {
            // Act
            var result = _controller.AgreeToTermsOfUbora(new AgreeToTermsOfUboraPostModel());

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void AgreeToTermsOfUbora_Post_Can_Execute_Command_When_Authorized()
        {
            ChangeAgreementToTermsOfUboraCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<ChangeAgreementToTermsOfUboraCommand>()))
                .Callback<ChangeAgreementToTermsOfUboraCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            AuthorizationServiceMock.Setup(a => a.AuthorizeAsync(User, null, Policies.CanChangeAgreementToTermsOfUbora))
                .ReturnsAsync(AuthorizationResult.Success);

            // Execute cases
            Case_Agreement();
            Case_Disagreement();

            // # Cases
            void Case_Agreement()
            {
                var postModel = new AgreeToTermsOfUboraPostModel
                {
                    IsAgreed = true
                };

                // Act
                var result = (ViewResult)_controller.AgreeToTermsOfUbora(postModel);

                // Assert
                executedCommand.IsAgreed.Should().BeTrue();
                executedCommand.ProjectId.Should().Be(ProjectId);
                executedCommand.Actor.UserId.Should().Be(UserId);
            }

            void Case_Disagreement()
            {
                var postModel = new AgreeToTermsOfUboraPostModel
                {
                    IsAgreed = false
                };

                // Act
                var result = (ViewResult)_controller.AgreeToTermsOfUbora(postModel);

                // Assert
                executedCommand.IsAgreed.Should().BeFalse();
                executedCommand.ProjectId.Should().Be(ProjectId);
                executedCommand.Actor.UserId.Should().Be(UserId);
            }
        }
    }
}
