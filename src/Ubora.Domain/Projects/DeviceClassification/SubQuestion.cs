using System;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SubQuestion : BaseQuestion
    {
        public Guid MainQuestionId { get; }
        public Guid ParentQuestionId { get; }

        public SubQuestion(Guid id, string text, Guid mainQuestionId, Guid parentQuestionId) : base(id, text)
        {
            MainQuestionId = mainQuestionId;
            ParentQuestionId = parentQuestionId;
        }
    }
}