using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Tasks;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class TaskTokenReplacerTests
    {
        private readonly TaskTokenReplacer _taskTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public TaskTokenReplacerTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _taskTokenReplacer = new TaskTokenReplacer(_queryProcessorMock.Object, _urlHelperMock.Object);
        }

        [Fact]
        public void Replaces_Task_Tokens_With_Anchor_Tags()
        {
            var task1Title = "<\"123\">";
            var task1 = new ProjectTask()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, task1Title);

            var task2Title = "task2Title";
            var task2 = new ProjectTask()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, task2Title);

            var text = $"test1 {StringTokens.Task(task1.Id)} test2 {StringTokens.Task(task2.Id)} test3";

            _queryProcessorMock.Setup(x => x.FindById<ProjectTask>(task1.Id))
                .Returns(task1);

            _queryProcessorMock.Setup(x => x.FindById<ProjectTask>(task2.Id))
                .Returns(task2);

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Assignments" && x.Controller == "Assignments")))
                .Returns("tasksLink");

            // Act
            var result = _taskTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = "test1 <a href=\"tasksLink\">&lt;&quot;123&quot;&gt;</a> test2 <a href=\"tasksLink\">task2Title</a> test3";

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
