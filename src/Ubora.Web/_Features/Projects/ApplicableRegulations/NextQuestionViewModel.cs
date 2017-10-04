using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class NextQuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Text { get; set; }
        public bool Answer { get; set; }
        public string Note { get; set; }
    }
}