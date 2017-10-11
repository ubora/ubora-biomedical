using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class LastQuestionViewModel
    {
        public Guid Id { get; set; }
       // public Guid? NextQuestionId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Text { get; set; }
        public bool Answer { get; set; }
    }
}
