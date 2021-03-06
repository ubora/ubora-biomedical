﻿using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;
using System.Text.Encodings.Web;

namespace Ubora.Web.Tests._Features._Shared
{
    public class ProjectTokenReplacerTests
    {
        private readonly ProjectTokenReplacer _userTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<HtmlEncoder> _htmlEncoderMock;

        public ProjectTokenReplacerTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _htmlEncoderMock = new Mock<HtmlEncoder>();
            _userTokenReplacer = new ProjectTokenReplacer(_queryProcessorMock.Object, _urlHelperMock.Object, _htmlEncoderMock.Object);
        }

        [Fact]
        public void Replaces_Project_Tokens_With_Anchor_Tags()
        {
            var project1 = new Project()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, "project1Title");

            var project2 = new Project()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, "project2Title");

            var text = $"test1 {StringTokens.Project(project1.Id)} test2 {StringTokens.Project(project2.Id)} test3";

            _queryProcessorMock.Setup(x => x.FindById<Project>(project1.Id))
                .Returns(project1);

            _queryProcessorMock.Setup(x => x.FindById<Project>(project2.Id))
                .Returns(project2);

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Dashboard" && x.Controller == "Dashboard" && x.Values.GetPropertyValue<Guid>("projectId") == project1.Id)))
                .Returns("project1link");

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Dashboard" && x.Controller == "Dashboard" && x.Values.GetPropertyValue<Guid>("projectId") == project2.Id)))
                .Returns("project2link");

            var encodedProject1Title = "encodedProject1Title";
            _htmlEncoderMock.Setup(x => x.Encode(project1.Title))
                .Returns(encodedProject1Title);

            var encodedProject2Title = "encodedProject2Title";
            _htmlEncoderMock.Setup(x => x.Encode(project2.Title))
                .Returns(encodedProject2Title);

            // Act
            var result = _userTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = $"test1 <a href=\"project1link\">{encodedProject1Title}</a> test2 <a href=\"project2link\">{encodedProject2Title}</a> test3";

            result.Should().Be(expected);
        }

        [Fact]
        public void Works_Without_Tokens_In_String()
        {
            // Act
            var result = _userTokenReplacer.ReplaceTokens("text");

            // Assert
            result.Should().Be("text");
        }
    }
}
