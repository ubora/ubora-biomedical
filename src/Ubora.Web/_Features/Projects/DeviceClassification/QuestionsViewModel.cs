using System;
using System.Collections.Generic;
using Ubora.Web._Features.Projects.DeviceClassification.Services;

namespace Ubora.Web._Features.Projects.DeviceClassification
{
    public class QuestionsViewModel
    {
        public IReadOnlyCollection<SubQuestion> Questions { get; set; }

        public Guid MainQuestionId { get; set; }
        public Guid ProjectId { get; internal set; }
    }
}
