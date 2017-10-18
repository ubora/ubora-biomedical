using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Web._Features.Projects.ApplicableRegulations;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.ApplicableRegulations
{
    public class NextQuestionViewModelFactoryTests
    {
        private Mock<Questionnaire> _questionnaireMock;

        [Fact]
        public void Create_Creates_Next_Question_View_Model_From_Questionnaire_Aggregate_And_QuestionId()
        {
            var questionId = Guid.NewGuid();
            var questionnaireId = Guid.NewGuid();
            var previousQuestionnaireId = Guid.NewGuid();
            var nextQuestionnaireId = Guid.NewGuid();
            var questionText = "Küsimus";
            var noteText = "noteText";

            var question = Mock.Of<Question>(x => x.QuestionText == questionText && x.NoteText == noteText)
                .Set(x => x.Answer, true);

            _questionnaireMock = new Mock<Questionnaire>();
            _questionnaireMock.Setup(x => x.FindQuestionOrThrow(questionId))
                .Returns(question);

            var aggregate = new ApplicableRegulationsQuestionnaireAggregate()
                .Set(x => x.Questionnaire, _questionnaireMock.Object)
                .Set(x => x.Id, questionnaireId);

            var nextQuestion = Mock.Of<Question>()
                .Set(x => x.Id, nextQuestionnaireId);
            _questionnaireMock.Setup(x => x.FindNextQuestionFromAnsweredQuestion(question))
                .Returns(nextQuestion);

            var previousQuestion = Mock.Of<Question>()
                .Set(x => x.Id, previousQuestionnaireId);
            _questionnaireMock.Setup(x => x.FindPreviousAnsweredQuestionFrom(question))
                .Returns(previousQuestion);

            var modelFactory = new NextQuestionViewModel.Factory();

            // Act
            var result = modelFactory.Create(aggregate, questionId);

            // Assert
            result.Id.Should().Be(questionId);
            result.Text.Should().Be(questionText);
            result.QuestionnaireId.Should().Be(questionnaireId);
            result.Answer.Should().Be(true);
            result.PreviousAnsweredQuestionId.Should().Be(previousQuestion.Id);
            result.NextQuestionId.Should().Be(nextQuestion.Id);
            result.Note.Should().Be(noteText);
        }

        [Fact]
        public void Create_When_Question_Has_No_Answer_Next_QuestionId_Should_Be_Null()
        {
            var questionId = Guid.NewGuid();
            var question = Mock.Of<Question>().Set(x => x.Answer, null);

            _questionnaireMock = new Mock<Questionnaire>();
            _questionnaireMock.Setup(x => x.FindQuestionOrThrow(questionId))
                .Returns(question);

            var aggregate = new ApplicableRegulationsQuestionnaireAggregate()
                .Set(x => x.Questionnaire, _questionnaireMock.Object);

            _questionnaireMock.Setup(x => x.FindNextQuestionFromAnsweredQuestion(question))
                .Throws(new AssertionFailedException($"{nameof(Questionnaire.FindNextQuestionFromAnsweredQuestion)} should not have been called!"));

            var modelFactory = new NextQuestionViewModel.Factory();

            // Act
            var result = modelFactory.Create(aggregate, questionId);

            // Assert
            result.NextQuestionId.Should().Be(null);
        }
    }
}