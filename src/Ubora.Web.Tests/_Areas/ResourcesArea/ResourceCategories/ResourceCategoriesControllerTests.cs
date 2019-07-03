using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Commands;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ResourcesArea.ResourceCategories;
using Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models;
using Xunit;

namespace Ubora.Web.Tests._Areas.ResourcesArea.ResourceCategories
{
    public class ResourceCategoriesControllerTests : UboraControllerTestsBase
    {
        private readonly ResourceCategoriesController _controller;
        private readonly Mock<ResourceCategoriesController> _controllerMock;

        public ResourceCategoriesControllerTests()
        {
            _controllerMock = new Mock<ResourceCategoriesController>
            {
                CallBase = true
            };
            _controller = _controllerMock.Object;

            SetUpForTest(_controller);
        }

        [Fact]
        public async Task Create_HttpPost_Authorizes()
        {
            // Act
            var result = await _controller.Create(new CreateResourceCategoryPostModel());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Edit_HttpPost_Authorizes()
        {
            // Act
            var result = await _controller.Edit(new EditResourceCategoryPostModel(), Mock.Of<EditResourceCategoryViewModel.Factory>());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Delete_HttpPost_Authorizes()
        {
            // Act
            var result = await _controller.Delete(new DeleteResourceCategoryPostModel(), Mock.Of<EditResourceCategoryViewModel.Factory>());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Create_HttpPost_HappyPath()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, null, Policies.CanManageResources))
                .ReturnsAsync(AuthorizationResult.Success);

            var postModel = AutoFixture.Create<CreateResourceCategoryPostModel>();

            CreateResourceCategoryCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<CreateResourceCategoryCommand>()))
                .Returns(CommandResult.Success)
                .Callback<CreateResourceCategoryCommand>(c => executedCommand = c);

            // Act
            var result = await _controller.Create(postModel);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();

            executedCommand.Actor.UserId.Should().Be(UserId);
            executedCommand.CategoryId.Should().NotBe(default(Guid));
            executedCommand.Description.Should().Be(postModel.Description);
            executedCommand.MenuPriority.Should().Be(postModel.MenuPriority);
            executedCommand.ParentCategoryId.Should().Be(postModel.ParentCategoryId);
        }

        [Fact]
        public async Task Edit_HttpPost_HappyPath()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, null, Policies.CanManageResources))
                .ReturnsAsync(AuthorizationResult.Success);

            var postModel = AutoFixture.Create<EditResourceCategoryPostModel>();

            EditResourceCategoryCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<EditResourceCategoryCommand>()))
                .Returns(CommandResult.Success)
                .Callback<EditResourceCategoryCommand>(c => executedCommand = c);

            // Act
            var result = await _controller.Edit(postModel, Mock.Of<EditResourceCategoryViewModel.Factory>());

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();

            executedCommand.Actor.UserId.Should().Be(UserId);
            executedCommand.CategoryId.Should().Be(postModel.CategoryId);
            executedCommand.Description.Should().Be(postModel.Description);
            executedCommand.MenuPriority.Should().Be(postModel.MenuPriority);
            executedCommand.ParentCategoryId.Should().Be(postModel.ParentCategoryId);
        }

        [Fact]
        public async Task Delete_HttpPost_HappyPath()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, null, Policies.CanManageResources))
                .ReturnsAsync(AuthorizationResult.Success);

            var postModel = AutoFixture.Create<DeleteResourceCategoryPostModel>();

            DeleteResourceCategoryCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<DeleteResourceCategoryCommand>()))
                .Returns(CommandResult.Success)
                .Callback<DeleteResourceCategoryCommand>(c => executedCommand = c);

            // Act
            var result = await _controller.Delete(postModel, Mock.Of<EditResourceCategoryViewModel.Factory>());

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();

            executedCommand.Actor.UserId.Should().Be(UserId);
            executedCommand.ResourceCategoryId.Should().Be(postModel.ResourceCategoryId);
        }

        [Fact]
        public async Task Create_HttpPost_Invalid_ModelState()
        {
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.Create(new CreateResourceCategoryPostModel());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Edit_HttpPost_Invalid_ModelState()
        {
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.Edit(new EditResourceCategoryPostModel(), Mock.Of<EditResourceCategoryViewModel.Factory>());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Delete_HttpPost_Invalid_ModelState()
        {
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.Delete(new DeleteResourceCategoryPostModel(), Mock.Of<EditResourceCategoryViewModel.Factory>());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }
    }
}
