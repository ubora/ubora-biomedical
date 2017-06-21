using Newtonsoft.Json;
using System;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class PairedMainQuestions : IMainQuestion<PairedMainQuestions>
    {
        public PairedMainQuestions NextMainQuestion { get; private set; }
        public Guid Id { get; private set; }
        public MainQuestion MainQuestionOne { get; private set; }
        public MainQuestion MainQuestionTwo { get; private set; }

        public PairedMainQuestions(PairedMainQuestions nextMainQuestion, MainQuestion mainQuestionOne, MainQuestion mainQuestionTwo)
        {
            Id = Guid.NewGuid();
            NextMainQuestion = nextMainQuestion;
            MainQuestionOne = mainQuestionOne;
            MainQuestionTwo = mainQuestionTwo;
        }

        public bool IsLastMainQuestion => NextMainQuestion == null;
    }
}
