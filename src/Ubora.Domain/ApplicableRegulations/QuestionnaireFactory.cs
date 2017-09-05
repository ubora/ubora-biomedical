using System;
using Ubora.Domain.ApplicableRegulations.Texts;

// ReSharper disable InconsistentNaming

namespace Ubora.Domain.ApplicableRegulations
{
    public static class QuestionnaireFactory
    {
        public static Questionnaire Create(Guid id)
        {
            var question5_1 = new Question(nameof(QuestionTexts.Q5_1), nextQuestion: null);
            var question5 = new Question(nameof(QuestionTexts.Q5), null, new[] { question5_1 });
            var question4_2 = new Question(nameof(QuestionTexts.Q4_2), question5);
            var question4_1_4 = new Question(nameof(QuestionTexts.Q4_1_4), question5);
            var question4_1_3 = new Question(nameof(QuestionTexts.Q4_1_3), question5);
            var question4_1_2 = new Question(nameof(QuestionTexts.Q4_1_2), question5);
            var question4_1_1 = new Question(nameof(QuestionTexts.Q4_1_1), question5);
            var question4_1 = new Question(nameof(QuestionTexts.Q4_1), question5, new[] { question4_1_1, question4_1_2, question4_1_3, question4_1_4 });
            var question4 = new Question(nameof(QuestionTexts.Q4), question5, new[] { question4_1, question4_2 });
            var question3 = new Question(nameof(QuestionTexts.Q3), question4);
            var question2_1_1 = new Question(nameof(QuestionTexts.Q2_1_1), question3);
            var question2_1 = new Question(nameof(QuestionTexts.Q2_1), question3, new[] { question2_1_1 });
            var question2 = new Question(nameof(QuestionTexts.Q2), question3, new[] { question2_1 });
            var question1 = new Question(nameof(QuestionTexts.Q1), question2);

            return new Questionnaire(id, question1);
        }
    }
}
