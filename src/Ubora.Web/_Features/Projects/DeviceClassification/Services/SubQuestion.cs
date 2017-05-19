using System;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public class SubQuestion : BaseQuestion
    {
        public Guid MainQuestionId { get; }
        public Guid ParentQuestionId { get; }
        public bool HasParent => MainQuestionId != null;

        public SubQuestion(Guid id, string text, Guid mainQuestionId, Guid parentQuestionId) : base(id, text)
        {
            MainQuestionId = mainQuestionId;
            ParentQuestionId = parentQuestionId;
        }
    }
}