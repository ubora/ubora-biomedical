using FluentAssertions;
using Ubora.Domain.Projects.DeviceClassification;
using Xunit;

namespace Ubora.Domain.Tests.Projects.DeviceClassification
{
    public class SpecialMainQuestionTests
    {
        [Fact]
        public void GetNote_Returns_Note_If_SpecialMainQuestion_Has_A_Note()
        {
            var specialMainQuestion = new SpecialMainQuestion("", null, new Note("note"));

            // Act
            var result = specialMainQuestion.GetNote();

            // Assert
            result.Should().Be("note");
        }

        [Fact]
        public void GetNote_Returns_Null_If_SpecialMainQuestion_Has_No_Note()
        {
            var specialMainQuestion = new SpecialMainQuestion("", null, note: null);

            // Act
            var result = specialMainQuestion.GetNote();

            // Assert
            result.Should().BeNull();
        }
    }
}
