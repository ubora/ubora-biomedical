using System;
using Ubora.Domain.Questionnaires.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class NextQuestionViewModel
    {
        public string Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public string PreviousAnsweredQuestionId { get; set; }
        public string NextQuestionId { get; set; }
        public string Note { get; set; }

        public class Factory
        {
            public NextQuestionViewModel Create(ApplicableRegulationsQuestionnaireAggregate questionnaireAggregate, string questionId)
            {
                var question = questionnaireAggregate.Questionnaire.FindQuestionOrThrow(questionId);
                var model = new NextQuestionViewModel
                {
                    Id = questionId,
                    Text = question.QuestionText,
                    QuestionnaireId = questionnaireAggregate.Id,
                    Answer = question.ChosenAnswerText,
                    Note = question.NoteText,
                    PreviousAnsweredQuestionId = questionnaireAggregate.Questionnaire.FindPreviousAnsweredQuestionFrom(question)?.Id,
                    NextQuestionId = question.IsAnswered ? questionnaireAggregate.Questionnaire.FindNextQuestionFromAnsweredQuestion(question)?.Id : null
                };
                return model;
            }
        }
    }
}