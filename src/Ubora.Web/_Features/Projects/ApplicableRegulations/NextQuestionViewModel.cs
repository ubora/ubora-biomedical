using System;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class NextQuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Text { get; set; }
    }
}