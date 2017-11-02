using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
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
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

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
        public void GetAllQuestions_Returns_All_Questions_In_Order()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            // Act
            var result = questionnaire.GetAllQuestions();

            // Assert
            result.Should()
                .ContainInOrder(question1, question2, question2_1, question2_1_1, question2_1_2, question2_2, question3, question4);
        }

        [Fact]
        public void FindNextUnansweredQuestion_Returns_Next_Unanswered_Question_And_All_Questions_Can_Be_Answered_Through_That_When_Iterated()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);
            var allQuestions = questionnaire.GetAllQuestions().ToList();

            // Act
            for (int i = 0; i < allQuestions.Count; i++)
            {
                questionnaire
                    .FindNextUnansweredQuestion()
                    .AnswerQuestion(true);
            }

            // Assert
            allQuestions.All(x => x.Answer == true).Should().BeTrue();
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Returns_Previous_Answered_Question()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            question2.AnswerQuestion(true);
            question2_1.AnswerQuestion(false);

            // Act
            var previousQuestion = questionnaire.FindPreviousAnsweredQuestionFrom(question2);

            // Assert
            previousQuestion.Should().Be(question1);
        }
        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Returns_Previous_Answered_Question_From_All_Questions()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            question2.AnswerQuestion(true);

            // Act
            var previousQuestion = questionnaire.FindPreviousAnsweredQuestionFrom(question3);

            // Assert
            previousQuestion.Should().Be(question2);
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Returns_Null_When_First_Question()
        {
            var question4 = new Question("4", null);
            var question3 = new Question("3", question4);
            var question2_2 = new Question("2_2", question3);
            var question2_1_2 = new Question("2_1_2", question3);
            var question2_1_1 = new Question("2_1_1", question3);
            var question2_1 = new Question("2_1", question3, new[] { question2_1_1, question2_1_2 });
            var question2 = new Question("2", question3, new[] { question2_1, question2_2 });
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);

            // Act
            var previousQuestion = questionnaire.FindPreviousAnsweredQuestionFrom(question1);

            // Assert
            previousQuestion.Should().Be(null);
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Thorws_When_Question_Is_Not_In_The_List()
        {
            var guestionNotFromTheList = new Question("1", null);

            var question2 = new Question("2", null);
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            question2.AnswerQuestion(true);

            // Act
            Action result = () =>
             {
                 questionnaire.FindPreviousAnsweredQuestionFrom(guestionNotFromTheList);
             };

            // Assert
            result.ShouldThrow<InvalidOperationException>();

        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Throws_When_Question_Is_Not_Answered()
        {
            var question2 = new Question("2", null);
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);

            // Act
            Action result = () =>
            {
                questionnaire.FindNextQuestionFromAnsweredQuestion(question2);
            };

            // Assert
            result.ShouldThrow<InvalidOperationException>().WithMessage("Given question is not answered.");
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Throws_When_Question_Is_Not_In_The_List()
        {
            var questionNotFromList = new Question("1", null);
            var question2 = new Question("2", null);
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            questionNotFromList.AnswerQuestion(true);

            // Act
            Action result = () =>
            {
                questionnaire.FindNextQuestionFromAnsweredQuestion(questionNotFromList);
            };

            // Assert
            result.ShouldThrow<InvalidOperationException>().WithMessage("Given question not found from this questionnaire.");
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Returns_Null_When_Last_Question()
        {
            var question2 = new Question("2", null);
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            question2.AnswerQuestion(true);

            // Act
            var previousQuestion = questionnaire.FindNextQuestionFromAnsweredQuestion(question2);

            // Assert
            previousQuestion.Should().Be(null);
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Returns_Next_Aswered_Question()
        {
            var question2 = new Question("2", null);
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            question2.AnswerQuestion(true);

            // Act
            var previousQuestion = questionnaire.FindNextQuestionFromAnsweredQuestion(question1);

            // Assert
            previousQuestion.Should().Be(question2);
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Returns_Next_Unasnwered_Question_When_There_Is_No_Answered_Questions_Left()
        {
            var question3 = new Question("3", null);
            var question2 = new Question("2", question3);
            var question1 = new Question("1", question2);

            var questionnaire = new Questionnaire(firstQuestion: question1);

            question1.AnswerQuestion(true);
            question2.AnswerQuestion(true);

            // Act
            var previousQuestion = questionnaire.FindNextQuestionFromAnsweredQuestion(question2);

            // Assert
            previousQuestion.Should().Be(question3);
        }
    }
}
