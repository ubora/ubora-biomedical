using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Repository;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class FileTokenReplacerTests
    {
        private readonly FileTokenReplacer _fileTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public FileTokenReplacerTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _fileTokenReplacer = new FileTokenReplacer(_queryProcessorMock.Object, _urlHelperMock.Object);
        }

        [Fact]
        public void Replaces_File_Tokens_With_Anchor_Tags()
        {
            var file1 = new ProjectFile()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.FileName, "<\"file1Name\">");

            var file2 = new ProjectFile()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.FileName, "file2Name");

            var text = $"test1 {StringTokens.File(file1.Id)} test2 {StringTokens.File(file2.Id)} test3";

            _queryProcessorMock.Setup(x => x.FindById<ProjectFile>(file1.Id))
                .Returns(file1);

            _queryProcessorMock.Setup(x => x.FindById<ProjectFile>(file2.Id))
                .Returns(file2);

            // Act
            var result = _fileTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = "test1 &lt;&quot;file1Name&quot;&gt; test2 file2Name test3";

            result.Should().Be(expected);
        }

        [Fact]
        public void Works_Without_Tokens_In_String()
        {
            // Act
            var result = _fileTokenReplacer.ReplaceTokens("text");

            // Assert
            result.Should().Be("text");
        }
    }
}
