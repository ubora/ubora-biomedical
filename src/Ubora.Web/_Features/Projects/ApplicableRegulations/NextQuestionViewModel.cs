using System;
using Ubora.Domain.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class NextQuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Text { get; set; }
        public bool? Answer { get; set; }
        public string Note { get; set; }
        public Guid? PreviousAnsweredQuestionId { get; set; }
        public Guid? NextQuestionId { get; set; }

        public class Factory
        {
            public NextQuestionViewModel Create(ApplicableRegulationsQuestionnaireAggregate questionnaireAggregate, Guid questionId)
            {
                var question = questionnaireAggregate.Questionnaire.FindQuestionOrThrow(questionId);
                var model = new NextQuestionViewModel
                {
                    Id = questionId,
                    Text = question.QuestionText,
                    QuestionnaireId = questionnaireAggregate.Id,
                    Answer = question.Answer,
                    Note = question.NoteText,
                    PreviousAnsweredQuestionId = questionnaireAggregate.Questionnaire.FindPreviousAnsweredQuestionFrom(question)?.Id,
                    NextQuestionId = question.Answer.HasValue ? questionnaireAggregate.Questionnaire.FindNextQuestionFromAnsweredQuestion(question)?.Id : null
                };
                return model;
            }
        }
    }
}