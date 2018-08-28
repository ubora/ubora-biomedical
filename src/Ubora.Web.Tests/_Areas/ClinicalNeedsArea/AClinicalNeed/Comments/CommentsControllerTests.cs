using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Comments;
using Ubora.Web._Components.Discussions.Models;
using Xunit;
using FluentAssertions;
using Marten;
using Ubora.Domain.Discussions;
using Ubora.Domain.Tests;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web.Tests._Areas.ClinicalNeedsArea.AClinicalNeed.Comments
{
    public class CommentsControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<CommentsController> _controllerMock;
        private readonly CommentsController _controller;

        public CommentsControllerTests()
        {
            _controllerMock = new Mock<CommentsController>(Mock.Of<IQuerySession>(), Mock.Of<ImageStorageProvider>())
            {
                CallBase = true
            };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);

            _controller.Set(c => c.Discussion, new Discussion());
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Failed()));
        }

        [Fact]
        public async Task AddComment_HttpPost_Returns_ForbidResult_When_Not_Authorized()
        {
            // Act
            var result = await _controller.AddComment(new AddCommentModel());

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task EditComment_HttpPost_Returns_ForbidResult_When_Not_Authorized()
        {
            var comment = TestComments.CreateDummy();
            var comments = new[]
            {
                comment
            }.ToImmutableList();

            var discussion = new Discussion().Set(d => d.Comments, comments);
            _controller.Set(c => c.Discussion, discussion);

            var model = new EditCommentModel
            {
                CommentId = comment.Id
            };

            // Act
            var result = await _controller.EditComment(model);

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task DeleteComment_HttpPost_Returns_ForbidResult_When_Not_Authorized()
        {
            var comment = TestComments.CreateDummy();
            var comments = new[]
            {
                comment
            }.ToImmutableList();

            var discussion = new Discussion().Set(d => d.Comments, comments);
            _controller.Set(c => c.Discussion, discussion);

            var model = new EditCommentModel
            {
                CommentId = comment.Id
            };

            // Act
            var result = await _controller.EditComment(model);

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }
    }
}
