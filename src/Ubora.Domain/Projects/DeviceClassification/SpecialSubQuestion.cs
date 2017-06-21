namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SpecialSubQuestion : BaseQuestion, ISubQuestion
    {
        public BaseQuestion ParentQuestion { get; private set; }

        public SpecialSubQuestion(string questionText, SpecialMainQuestion parentSpecialMainQuestion) : base(questionText)
        {
            ParentQuestion = parentSpecialMainQuestion;
        }
    }
}
