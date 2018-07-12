using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain;
using Ubora.Domain.Resources;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web._Areas.ResourcesArea.ResourcePages.Models;
using Xunit;

namespace Ubora.Web.Tests._Areas.ResourcesArea.ResourcePages
{
    public class ResourcePagesControllerTests : UboraControllerTestsBase
    {
        private readonly ResourcePagesController _controller;
        private readonly Mock<ResourcePagesController> _controllerMock;
        private static readonly Fixture AutoFixture = new Fixture();

        public ResourcePagesControllerTests()
        {
            _controllerMock = new Mock<ResourcePagesController>
            {
                CallBase = true
            };
            _controller = _controllerMock.Object;

            SetUpForTest(_controller);
        }

        [Fact]
        public async Task AddFile_HttpPost_Authorizes()
        {
            // Act
            var result = await _controller.AddFile(new AddResourceFilePostModel());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Edit_HttpPost_Authorizes()
        {
            // Act
            var result = await _controller.Edit(new ResourceEditPostModel(), Mock.Of<ResourceEditViewModel.Factory>());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Delete_HttpPost_Authorizes()
        {
            // Act
            var result = await _controller.Delete(new DeleteResourcePagePostModel());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task AddFile_HttpPost_Invalid_ModelState()
        {
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.AddFile(new AddResourceFilePostModel());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Edit_HttpPost_Invalid_ModelState()
        {
            _controllerMock.Setup(c => c.ResourcePage).Returns(new ResourcePage().Set(x => x.Body, new QuillDelta()));
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.Edit(new ResourceEditPostModel(), Mock.Of<ResourceEditViewModel.Factory>());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Delete_HttpPost_Invalid_ModelState()
        {
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Success()));
            _controller.ModelState.AddModelError("", "dummy");

            // Act
            var result = await _controller.Delete(new DeleteResourcePagePostModel());

            // Assert
            result.Should().NotBeOfType<RedirectToActionResult>();
        }
    }
}
