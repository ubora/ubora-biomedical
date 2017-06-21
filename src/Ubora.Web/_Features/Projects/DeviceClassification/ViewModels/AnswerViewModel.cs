using System;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.DeviceClassification.ViewModels
{
    public class AnswerViewModel
    {
        [NotDefault]
        public Guid NextQuestionId { get; set; }
        public Guid? PairedMainQuestionsId { get; set; }
        public string QuestionText { get; set; }
    }
}
