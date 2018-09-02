using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ClinicalNeedsArea.IndicateClinicalNeed;
using Ubora.Web._Areas.ClinicalNeedsArea.IndicateClinicalNeed.Models;
using Xunit;

namespace Ubora.Web.Tests._Areas.ClinicalNeedsArea.IndicateClinicalNeed
{
    public class IndicateClinicalNeedControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<IndicateClinicalNeedController> _controllerMock;
        private readonly IndicateClinicalNeedController _controller;

        public IndicateClinicalNeedControllerTests()
        {
            _controllerMock = new Mock<IndicateClinicalNeedController> { CallBase = true };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);
        }

        [Fact]
        public void Controller_Has_Authorize_Attibute()
        {
            var authorizeAttribute =  typeof(IndicateClinicalNeedController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: false)
                .Single() as AuthorizeAttribute;

            authorizeAttribute.Policy.Should().Be(Policies.CanIndicateClinicalNeeds);
        }

        [Fact]
        public void Finalize_Does_Not_Execute_Command_When_ModelState_Is_Invalid()
        {
            var model = new StepTwoModel();

            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = (ViewResult)_controller.Finalize(model);

            // Assert
            result.ViewName.Should().Be(nameof(_controller.StepTwo));
        }

        [Fact]
        public void Finalize_Executes_Command_When_HappyPath()
        {
            var model = new StepTwoModel
            {
                Title = "title",
                AreaOfUsageTag = "area",
                ClinicalNeedTag = "clinical",
                Description = "{description}",
                Keywords = "keywords",
                PotentialTechnologyTag = "tech"
            };

            IndicateClinicalNeedCommand executedCommand = null;
            CommandProcessorMock
                .Setup(c => c.Execute(It.IsAny<IndicateClinicalNeedCommand>()))
                .Callback<IndicateClinicalNeedCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            var result = _controller.Finalize(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).RouteValues["clinicalNeedId"].Should().Be(executedCommand.ClinicalNeedId);

            using (new AssertionScope())
            {
                executedCommand.ClinicalNeedId.Should().NotBe(default(Guid));
                executedCommand.Actor.UserId.Should().Be(UserId);
                executedCommand.Title.Should().Be("title");
                executedCommand.AreaOfUsageTag.Should().Be("area");
                executedCommand.ClinicalNeedTag.Should().Be("clinical");
                executedCommand.Description.Should().Be(new QuillDelta("{description}"));
                executedCommand.Keywords.Should().Be("keywords");
                executedCommand.PotentialTechnologyTag.Should().Be("tech");
            }
        }
    }
}