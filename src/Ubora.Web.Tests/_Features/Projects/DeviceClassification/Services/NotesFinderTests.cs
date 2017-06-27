using FluentAssertions;
using System.Collections.Generic;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.DeviceClassification.Services
{
    public class NotesFinderTests
    {
        private NotesFinder _notesFinder;

        public NotesFinderTests()
        {
            _notesFinder = new NotesFinder();
        }

        [Fact]
        public void GetNotes_Returns_First_MainQuestion_Note_If_Second_Note_Is_The_Same_As_First()
        {
            var note = new Note("note1");
            var mainQuestionOne = new MainQuestion("", note);
            var mainQuestionTwo = new MainQuestion("", note);
            var pair = new PairedMainQuestions(null, mainQuestionOne, mainQuestionTwo);

            var expectedResult = new string[] { "note1" };

            // Act
            var result = _notesFinder.GetNotes(pair);

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetNotes_Returns_Notes_If_MainQuestions_Have_Notes()
        {
            var mainQuestionOne = new MainQuestion("", null);
            var mainQuestionTwo = new MainQuestion("", new Note("note2"));
            var pair = new PairedMainQuestions(null, mainQuestionOne, mainQuestionTwo);

            var expectedResult = new string[] { "note2" };

            // Act
            var result = _notesFinder.GetNotes(pair);

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetNote_Returns_Note_If_SpecialMainQuestion_Has_A_Note()
        {
            var specialMainQuestion = new SpecialMainQuestion("", null, new Note("note"));

            // Act
            var result = _notesFinder.GetNote(specialMainQuestion);

            // Assert
            result.Should().Be("note");
        }

        [Fact]
        public void GetNote_Returns_Null_If_SpecialMainQuestion_Has_No_Note()
        {
            var specialMainQuestion = new SpecialMainQuestion("", null, note: null);

            // Act
            var result = _notesFinder.GetNote(specialMainQuestion);

            // Assert
            result.Should().BeNull();
        }

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
            var result = _notesFinder.GetNotes(questions);

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
