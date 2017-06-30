using FluentAssertions;
using System.Collections.Generic;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.DeviceClassification.Services
{
    public class NotesFinderTests
    {
        [Fact]
        public void GetNotes_Returns_Unique_Notes_If_Questions_Have_Notes()
        {
            var note = new Note("note2");
            var questions = new List<BaseQuestion>
            {
                new TestBaseQuestion("", new Note("note1")),
                new TestBaseQuestion("", null),
                new TestBaseQuestion("", note),
                new TestBaseQuestion("", note),
            };

            var expectedResult = new[] { "note1", "note2" };

            // Act
            var result = questions.GetNotes();

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }
    }

    internal class TestBaseQuestion : BaseQuestion
    {
        public TestBaseQuestion(string questionText, Note note) : base(questionText, note)
        {
        }
    }
}
