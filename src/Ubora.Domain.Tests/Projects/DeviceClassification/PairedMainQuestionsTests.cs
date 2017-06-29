using FluentAssertions;
using Ubora.Domain.Projects.DeviceClassification;
using Xunit;

namespace Ubora.Domain.Tests.Projects.DeviceClassification
{
    public class PairedMainQuestionsTests
    {
        [Fact]
        public void GetNotes_Returns_First_MainQuestion_Note_If_Second_Note_Is_The_Same_As_First()
        {
            var note = new Note("note1");
            var mainQuestionOne = new MainQuestion("", note);
            var mainQuestionTwo = new MainQuestion("", note);
            var pair = new PairedMainQuestions(null, mainQuestionOne, mainQuestionTwo);

            var expectedResult = new string[] { "note1" };

            // Act
            var result = pair.GetNotes();

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
            var result = pair.GetNotes();

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }
    }
}
