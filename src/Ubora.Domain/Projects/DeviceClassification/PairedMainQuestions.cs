using System;
using System.Collections.Generic;

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

        public virtual IEnumerable<string> GetNotes()
        {
            if (MainQuestionOne != null && MainQuestionOne.Note != null
                && MainQuestionTwo != null && MainQuestionTwo.Note != null
                && MainQuestionOne.Note.Id == MainQuestionTwo.Note.Id)
            {
                return new List<string> { MainQuestionOne.Note.Value };
            }

            var notes = new List<string>();

            if (MainQuestionOne != null && MainQuestionOne.Note != null)
            {
                notes.Add(MainQuestionOne.Note.Value);
            }

            if (MainQuestionTwo != null && MainQuestionTwo.Note != null)
            {
                notes.Add(MainQuestionTwo.Note.Value);
            }

            return notes;
        }
    }
}
