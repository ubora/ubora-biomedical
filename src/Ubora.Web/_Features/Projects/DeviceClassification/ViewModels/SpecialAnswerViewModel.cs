using System;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.DeviceClassification.ViewModels
{
    public class SpecialAnswerViewModel
    {
        [NotDefault]
        public Guid NextQuestionId { get; set; }
        public Guid? MainQuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}
