namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SpecialSubQuestion : BaseQuestion, ISubQuestion
    {
        public BaseQuestion ParentQuestion { get; private set; }

        public SpecialSubQuestion(string questionText, SpecialMainQuestion parentSpecialMainQuestion, Note note = null) : base(questionText, note)
        {
            ParentQuestion = parentSpecialMainQuestion;
        }
    }
}
