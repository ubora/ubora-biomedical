using System;
using System.Collections.Generic;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web._Features.Projects.DeviceClassification.ViewModels
{
    public class SpecialSubQuestionsViewModel
    {
        public IReadOnlyCollection<SpecialSubQuestion> Questions { get; internal set; }
        public Guid MainQuestionId { get; internal set; }
    }
}
