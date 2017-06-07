using System;

namespace Ubora.Web._Features.Projects.DeviceClassification.ViewModels
{
    public class MainQuestionAnswerViewModel
    {
        public Guid MainQuestionId { get; set; }
        public Guid PairedQuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}
