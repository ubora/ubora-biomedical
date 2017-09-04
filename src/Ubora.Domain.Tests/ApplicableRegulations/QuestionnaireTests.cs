using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.ApplicableRegulations;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Ubora.Domain.Tests.ApplicableRegulations
{
    public class QuestionnaireTests
    {
        [Fact]
        public void FindQuestionOrThrow_Finds_Question_From_Questionnaire_Tree()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new [] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(id: Guid.NewGuid(), firstQuestion: question1);

            // Act & Assert
            questionnaire.FindQuestionOrThrow(question1.Id).Should().Be(question1);
            questionnaire.FindQuestionOrThrow(question2.Id).Should().Be(question2);
            questionnaire.FindQuestionOrThrow(question2_1.Id).Should().Be(question2_1);
            questionnaire.FindQuestionOrThrow(question2_1_1.Id).Should().Be(question2_1_1);
            questionnaire.FindQuestionOrThrow(question2_1_2.Id).Should().Be(question2_1_2);
            questionnaire.FindQuestionOrThrow(question2_2.Id).Should().Be(question2_2);
            questionnaire.FindQuestionOrThrow(question3.Id).Should().Be(question3);
            questionnaire.FindQuestionOrThrow(question4.Id).Should().Be(question4);
        }

        [Fact]
        public void GetAllQuestions_Returns_All_Questions()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var allQuestions = new List<Question>
            {
                question1,
                question2,
                question2_1,
                question2_1_1,
                question2_1_2,
                question2_2,
                question3,
                question4
            };

            var questionnaire = new Questionnaire(id: Guid.NewGuid(), firstQuestion: question1);

            // Act
            var result = questionnaire.GetAllQuestions();

            // Assert
            result.Should().BeEquivalentTo(allQuestions);
        }

        [Fact]
        public void GetNextUnanswered_Returns_Next_Unanswered_Question()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new [] { question2_1_1, question2_1_2} );
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(id: Guid.NewGuid(), firstQuestion: question1);
            var allQuestions = questionnaire.GetAllQuestions().ToList();

            // Act
            for (int i = 0; i < allQuestions.Count; i++)
            {
                questionnaire.GetNextUnanswered().Answer = true;
            }

            // Assert
            allQuestions.All(x => x.Answer == true).Should().BeTrue();
        }
    }
}
