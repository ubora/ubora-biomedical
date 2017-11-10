using System;
using FluentAssertions;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Ubora.Domain.Tests.Questionnaires.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireTreeTests
    {
        [Fact]
        public void FindQuestionOrThrow_Finds_Question_From_Questionnaire_Tree()
        {
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions);

            // Act & Assert
            foreach (var q in questions)
            {
                questionnaire.FindQuestionOrThrow(q.Id).Should().Be(q);
            }
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Returns_Previous_Answered_Question()
        {
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions);

            questions[0].ChooseAnswer("y", DateTime.UtcNow);
            questions[1].ChooseAnswer("y", DateTime.UtcNow);
            questions[2].ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var previousQuestion = questionnaire.FindPreviousAnsweredQuestionFrom(questions[2]);

            // Assert
            previousQuestion.Should().Be(questions[1]);
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Returns_Previous_Answered_Question_From_All_Questions()
        {
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions);

            questions[0].ChooseAnswer("y", DateTime.UtcNow);
            questions[1].ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var previousQuestion = questionnaire.FindPreviousAnsweredQuestionFrom(questions[5]);

            // Assert
            previousQuestion.Should().Be(questions[1]);
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Returns_Null_When_First_Question()
        {
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions); ;

            var firstQuestion = questions[0];
            firstQuestion.ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var previousQuestion = questionnaire.FindPreviousAnsweredQuestionFrom(firstQuestion);

            // Assert
            previousQuestion.Should().Be(null);
        }

        [Fact]
        public void FindPreviousAnsweredQuestionFrom_Thorws_When_Question_Is_Not_In_The_List()
        {
            var guestionNotFromTheList = new Question("1", new[] { new Answer("y", null), new Answer("n", null) });

            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions: CreateQuestions());

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
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions);

            questions[0].ChooseAnswer("y", DateTime.UtcNow);

            // Act
            Action result = () =>
            {
                questionnaire.FindNextQuestionFromAnsweredQuestion(questions[1]);
            };

            // Assert
            result.ShouldThrow<InvalidOperationException>().WithMessage("Given question is not answered.");
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Throws_When_Question_Is_Not_In_The_List()
        {
            var guestionNotFromTheList = new Question("1", new[] { new Answer("y", null), new Answer("n", null) });
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions: CreateQuestions());

            guestionNotFromTheList.ChooseAnswer("y", DateTime.UtcNow);

            // Act
            Action result = () =>
            {
                questionnaire.FindNextQuestionFromAnsweredQuestion(guestionNotFromTheList);
            };

            // Assert
            result.ShouldThrow<InvalidOperationException>().WithMessage("Given question not found from this questionnaire.");
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Returns_Null_When_Last_Question()
        {
            var onlyQuestion = new Question("1", new[] { new Answer("y", null), new Answer("n", null) });
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(new [] { onlyQuestion });

            onlyQuestion.ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var previousQuestion = questionnaire.FindNextQuestionFromAnsweredQuestion(onlyQuestion);

            // Assert
            previousQuestion.Should().Be(null);
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Returns_Next_Aswered_Question()
        {
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions);

            questions[0].ChooseAnswer("y", DateTime.UtcNow);
            questions[1].ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var previousQuestion = questionnaire.FindNextQuestionFromAnsweredQuestion(questions[0]);

            // Assert
            previousQuestion.Should().Be(questions[1]);
        }

        [Fact]
        public void FindNextQuestionFromAnsweredQuestion_Returns_Next_Unasnwered_Question_When_There_Is_No_Answered_Questions_Left()
        {
            var questions = CreateQuestions();
            var questionnaire = new ApplicableRegulationsQuestionnaireTree(questions);

            questions[0].ChooseAnswer("y", DateTime.UtcNow);
            questions[1].ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var previousQuestion = questionnaire.FindNextQuestionFromAnsweredQuestion(questions[1]);

            // Assert
            previousQuestion.Should().Be(questions[2]);
        }

        private static Question[] CreateQuestions()
        {
            var question3 = new Question("3", new[] {new Answer("y", null), new Answer("n", null)});
            var question2_2 = new Question("2_2", new[] {new Answer("y", "3"), new Answer("n", "3")});
            var question2_1_2 = new Question("2_1_2", new[] {new Answer("y", "2_2"), new Answer("n", "3")});
            var question2_1_1 = new Question("2_1_1", new[] {new Answer("y", "2_1_2"), new Answer("n", "2_1_2")});
            var question2_1 = new Question("2_1", new[] {new Answer("y", "2_1_1"), new Answer("n", "2_1_1")});
            var question2 = new Question("2", new[] {new Answer("y", "2_1"), new Answer("n", "3")});
            var question1 = new Question("1", new[] {new Answer("y", "2"), new Answer("n", "2")});

            var questions = new[]
            {
                question1,
                question2,
                question2_1,
                question2_1_1,
                question2_1_2,
                question2_2,
                question3
            };
            return questions;
        }
    }
}
