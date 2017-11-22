using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Text.Encodings.Web;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Assignments;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class TaskTokenReplacerTests
    {
        private readonly TaskTokenReplacer _taskTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<HtmlEncoder> _htmlEncoderMock;

        public TaskTokenReplacerTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _htmlEncoderMock = new Mock<HtmlEncoder>();
            _taskTokenReplacer = new TaskTokenReplacer(_queryProcessorMock.Object, _urlHelperMock.Object, _htmlEncoderMock.Object);
        }

        [Fact]
        public void Replaces_Task_Tokens_With_Anchor_Tags()
        {
            var task1Title = "task1Title";
            var task1 = new Assignment()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, task1Title);

            var task2Title = "task2Title";
            var task2 = new Assignment()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, task2Title);

            var text = $"test1 {StringTokens.Task(task1.Id)} test2 {StringTokens.Task(task2.Id)} test3";

            _queryProcessorMock.Setup(x => x.FindById<Assignment>(task1.Id))
                .Returns(task1);

            _queryProcessorMock.Setup(x => x.FindById<Assignment>(task2.Id))
                .Returns(task2);

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Edit" && x.Controller == "Assignments")))
                .Returns("tasksLink");

            var encodedTask1Title = "encodedTask1Title";
            _htmlEncoderMock.Setup(x => x.Encode(task1.Title))
                .Returns(encodedTask1Title);

            var encodedTask2Title = "encodedTask2Title";
            _htmlEncoderMock.Setup(x => x.Encode(task2.Title))
                .Returns(encodedTask2Title);

            // Act
            var result = _taskTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = $"test1 <a href=\"tasksLink\">{encodedTask1Title}</a> test2 <a href=\"tasksLink\">{encodedTask2Title}</a> test3";

            result.Should().Be(expected);
        }

        [Fact]
        public void Works_Without_Tokens_In_String()
        {
            // Act
            var result = _taskTokenReplacer.ReplaceTokens("text");

            // Assert
            result.Should().Be("text");
        }
    }
}
