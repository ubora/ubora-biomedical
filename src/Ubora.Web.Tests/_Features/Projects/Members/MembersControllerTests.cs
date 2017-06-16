using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Projects.Members;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Members
{
    public class MembersControllerTests : ProjectControllerTestsBase
    {
        private Mock<ICommandQueryProcessor> _processorMock;
        private MembersController _membersController;
        private Mock<FakeSignInManager> _signInManagerMock;

        public MembersControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _membersController = new MembersController(_processorMock.Object, _signInManagerMock.Object);
            SetProjectAndUserContext(_membersController);
        }

        [Fact]
        public void RemoveMember_Removes_Member_From_Project()
        {
            var viewModel = new RemoveMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            _processorMock.Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_membersController.RemoveMember(viewModel);

            // Assert
            result.ActionName.Should().Be(nameof(MembersController.Members));
        }

        [Fact]
        public void RemoveMember_Returns_Message_If_Command_Failed()
        {
            var viewModel = new RemoveMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            _processorMock.Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.RemoveMember(viewModel);

            // Assert
            ModelState.ErrorCount.Should().Be(1);
        }
    }
}
