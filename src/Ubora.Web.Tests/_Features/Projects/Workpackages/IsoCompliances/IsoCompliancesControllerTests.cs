using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.IsoCompliances
{
    public class IsoCompliancesControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<IsoCompliancesController> _controllerMock;
        private readonly IsoCompliancesController _controller;

        public IsoCompliancesControllerTests()
        {
            _controllerMock = new Mock<IsoCompliancesController>(Mock.Of<IndexViewModel.Factory>())
            {
                CallBase = true
            };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);
        }

        [Fact]
        public void AddIsoStandard_HttpPost_Invalid_ModelState()
        {
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = _controller.AddIsoStandard(new AddIsoStandardPostModel());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void AddIsoStandard_HttpPost_HappyPath()
        {
            var model = new AddIsoStandardPostModel
            {
                Title = "testTitle",
                ShortDescription = "testDescription",
                Link = "https://www.google.com/"
            };

            AddIsoStandardCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<AddIsoStandardCommand>()))
                .Callback<AddIsoStandardCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            var result = _controller.AddIsoStandard(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();

            using (new AssertionScope())
            {
                executedCommand.Title.Should().Be(model.Title);
                executedCommand.ShortDescription.Should().Be(model.ShortDescription);
                executedCommand.Link.ToString().Should().Be(model.Link);
            }
        }

        [Fact]
        public void RemoveIsoStandard_HttpPost_Invalid_ModelState()
        {
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = _controller.RemoveIsoStandard(new RemoveIsoStandardCommand());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void RemoveIsoStandard_HttpPost_HappyPath()
        {
            var isoStandardId = Guid.NewGuid();

            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, isoStandardId, Policies.CanRemoveIsoStandardFromComplianceChecklist))
                .ReturnsAsync(AuthorizationResult.Success());

            var expectedExecutedCommand = new RemoveIsoStandardCommand
            {
                IsoStandardId = isoStandardId
            };
            CommandProcessorMock
                .Setup(x => x.Execute(expectedExecutedCommand))
                .Returns(CommandResult.Success);

            // Act
            var result = _controller.RemoveIsoStandard(expectedExecutedCommand);

            // Assert

            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void MarkAsCompliant_HttpPost_Invalid_ModelState()
        {
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = _controller.MarkAsCompliant(new MarkIsoStandardAsCompliantCommand());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void MarkAsCompliant_HttpPost_HappyPath()
        {
            var expectedExecutedCommand = new MarkIsoStandardAsCompliantCommand();
            CommandProcessorMock
                .Setup(x => x.Execute(expectedExecutedCommand))
                .Returns(CommandResult.Success);

            // Act
            var result = _controller.MarkAsCompliant(expectedExecutedCommand);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void MarkAsNoncompliant_HttpPost_Invalid_ModelState()
        {
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = _controller.MarkAsNoncompliant(new MarkIsoStandardAsNoncompliantCommand());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void MarkAsNoncompliant_HttpPost_HappyPath()
        {
            var expectedExecutedCommand = new MarkIsoStandardAsNoncompliantCommand();
            CommandProcessorMock
                .Setup(x => x.Execute(expectedExecutedCommand))
                .Returns(CommandResult.Success);

            // Act
            var result = _controller.MarkAsNoncompliant(expectedExecutedCommand);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
