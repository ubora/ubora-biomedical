using System;

namespace Ubora.Web._Features.Projects.DeviceClassification
{
    public class MainQuestionViewModel
    {
        public Guid MainQuestionId { get; set; }
        public string MainQuestionText { get; set; }
        public Guid ProjectId { get; internal set; }
    }
}
