using System;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public class MainQuestion : BaseQuestion
    {
        public Guid? NextMainQuestion { get; }
        public MainQuestion(Guid id, string text, Guid? nextMainQuestion) : base(id, text)
        {
            NextMainQuestion = nextMainQuestion;
        }

        public bool IsLastMainQuestion => NextMainQuestion == null;
    }
}