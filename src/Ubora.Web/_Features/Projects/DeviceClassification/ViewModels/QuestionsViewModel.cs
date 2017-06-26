using System;
using System.Collections.Generic;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web._Features.Projects.DeviceClassification.ViewModels
{
    public class QuestionsViewModel
    {
        public IReadOnlyCollection<SubQuestion> Questions { get; set; }
        public Guid PairedMainQuestionsId { get; set; }
        public List<string> Notes { get; set; }
    }
}
