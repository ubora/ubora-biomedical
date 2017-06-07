namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SubQuestion : BaseQuestion, ISubQuestion
    {
        public PairedMainQuestions PairedMainQuestions { get; private set; }
        public BaseQuestion ParentQuestion { get; private set; }

        public SubQuestion(string questionText, PairedMainQuestions pairedMainQuestions, BaseQuestion parentQuestion) : base(questionText)
        {
            PairedMainQuestions = pairedMainQuestions;
            ParentQuestion = parentQuestion;
        }
    }
}