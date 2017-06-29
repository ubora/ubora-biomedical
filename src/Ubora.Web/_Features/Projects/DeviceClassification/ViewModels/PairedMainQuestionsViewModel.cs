using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.DeviceClassification.ViewModels
{
    public class PairedMainQuestionsViewModel
    {
        public Guid PairedQuestionId { get; set; }
        public Guid MainQuestionOneId { get; set; }
        public string MainQuestionOne { get; set; }
        public Guid MainQuestionTwoId { get; set; }
        public string MainQuestionTwo { get; set; }
        public IEnumerable<string> Notes { get; set; }
    }
}
