using System;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SpecialMainQuestion : BaseQuestion, IMainQuestion<SpecialMainQuestion>
    {
        public SpecialMainQuestion NextMainQuestion { get; private set; }
        public bool IsLastMainQuestion => NextMainQuestion == null;

        public SpecialMainQuestion(string questionText, SpecialMainQuestion nextMainQuestion, Note note = null) : base(questionText, note)
        {
            NextMainQuestion = nextMainQuestion;
        }
    }
}
