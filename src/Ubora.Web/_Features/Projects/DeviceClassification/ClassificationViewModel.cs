using System;

namespace Ubora.Web._Features.Projects.DeviceClassification
{
    public class ClassificationViewModel
    {
        public string ClassificationText { get; set; }
        public Guid ClassificationId { get; set; }
        public Guid MainQuestionId { get; set; }
    }
}
