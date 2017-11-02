namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public static class QuestionnaireTreeFactory
    {
        public static QuestionnaireTree Create()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", "q2"),
                    new Answer("n", "q2")
                }),
                new Question("q2", new[]
                {
                    new Answer("y", "q2_1"),
                    new Answer("n", "q3")
                }),
                new Question("q2_1", new[]
                {
                    new Answer("y", "q2_1_1"),
                    new Answer("n", "q3")
                }),
                new Question("q2_1_1", new[]
                {
                    new Answer("y", "q3"),
                    new Answer("n", "q3")
                }),
                new Question("q3", new[]
                {
                    new Answer("y", "q4"),
                    new Answer("n", "q4")
                }),
                new Question("q4", new[]
                {
                    new Answer("y", "q4_1"),
                    new Answer("n", "q5")
                }),
                new Question("q4_1", new[]
                {
                    new Answer("y", "q4_1_1"),
                    new Answer("n", "q4_2")
                }),
                new Question("q4_1_1", new[]
                {
                    new Answer("y", "q4_1_2"),
                    new Answer("n", "q4_1_2")
                }),
                new Question("q4_1_2", new[]
                {
                    new Answer("y", "q4_1_3"),
                    new Answer("n", "q4_1_3")
                }),
                new Question("q4_1_3", new[]
                {
                    new Answer("y", "q4_1_4"),
                    new Answer("n", "q4_1_4")
                }),
                new Question("q4_1_4", new[]
                {
                    new Answer("y", "q5"),
                    new Answer("n", "q5")
                }),
                new Question("q4_2", new[]
                {
                    new Answer("y", "q5"),
                    new Answer("n", "q5")
                }),
                new Question("q5", new[]
                {
                    new Answer("y", "q5_1"),
                    new Answer("n", null)
                }),
                new Question("q5_1", new[]
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            return new QuestionnaireTree(questions);
        }
    }
}
