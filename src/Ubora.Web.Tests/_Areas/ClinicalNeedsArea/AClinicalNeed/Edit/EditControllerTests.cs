using System;
using AutoFixture;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Moq;
using Ubora.Domain;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Edit;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Edit.Models;
using Xunit;

namespace Ubora.Web.Tests._Areas.ClinicalNeedsArea.AClinicalNeed.Edit
{
    public class EditControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<EditController> _controllerMock;
        private readonly Mock<INodeServices> _nodeServicesMock;
        private readonly EditController _controller;

        public EditControllerTests()
        {
            _nodeServicesMock = new Mock<INodeServices>();
            _controllerMock = new Mock<EditController>(_nodeServicesMock.Object)
            {
                CallBase = true
            };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);

            _controller.Set(c => c.ClinicalNeed, new ClinicalNeed());
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Failed()));
        }

        [Fact]
        public async Task Edit_HttpPost_Returns_ForbidResult_When_Not_Authorized()
        {
            // Act
            var result = await _controller.Edit(new EditClinicalNeedPostModel());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Edit_HttpPost_Does_Not_Execute_Command_When_ModelState_Is_Invalid()
        {
            AuthorizationServiceMock
                .Setup(a => a.AuthorizeAsync(User, It.IsAny<Guid>(), Policies.CanEditClinicalNeed))
                .ReturnsAsync(AuthorizationResult.Success);

            var expectedResult = new ViewResult();

            _controllerMock
                .Setup(c => c.Edit())
                .ReturnsAsync(expectedResult);

            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.Edit(new EditClinicalNeedPostModel());

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Edit_HttpPost_Executes_Command_When_Authorized()
        {
            var clinicalNeed = new ClinicalNeed().Set(cn => cn.Id, Guid.NewGuid());

            _controller.Set(c => c.ClinicalNeed, clinicalNeed);

            AuthorizationServiceMock
                .Setup(a => a.AuthorizeAsync(User, clinicalNeed.Id, Policies.CanEditClinicalNeed))
                .ReturnsAsync(AuthorizationResult.Success);

            EditClinicalNeedCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<EditClinicalNeedCommand>()))
                .Callback<EditClinicalNeedCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var postModel = AutoFixture.Create<EditClinicalNeedPostModel>();

            // Act
            var result = await _controller.Edit(postModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();

            using (new AssertionScope())
            {
                executedCommand.ClinicalNeedId.Should().Be(clinicalNeed.Id);
                executedCommand.Title.Should().Be(postModel.Title);
                executedCommand.AreaOfUsageTag.Should().Be(postModel.AreaOfUsageTag);
                executedCommand.ClinicalNeedTag.Should().Be(postModel.ClinicalNeedTag);
                executedCommand.Description.Should().Be(new QuillDelta(postModel.DescriptionQuillDelta));
                executedCommand.Keywords.Should().Be(postModel.Keywords);
                executedCommand.PotentialTechnologyTag.Should().Be(postModel.PotentialTechnologyTag);
            }
        }
    }
}
