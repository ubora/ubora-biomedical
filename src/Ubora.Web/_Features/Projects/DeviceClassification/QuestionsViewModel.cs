using System;
using System.Collections.Generic;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web._Features.Projects.DeviceClassification
{
    public class QuestionsViewModel
    {
        public IReadOnlyCollection<SubQuestion> Questions { get; set; }

        public Guid MainQuestionId { get; set; }
    }
}
