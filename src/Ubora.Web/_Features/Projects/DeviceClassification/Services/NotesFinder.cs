using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public class NotesFinder
    {
        public List<string> GetNotes(PairedMainQuestions pairedMainQuestions)
        {
            if (pairedMainQuestions.MainQuestionOne != null && pairedMainQuestions.MainQuestionOne.Note != null
                && pairedMainQuestions.MainQuestionTwo != null && pairedMainQuestions.MainQuestionTwo.Note != null
                && pairedMainQuestions.MainQuestionOne.Note.Id == pairedMainQuestions.MainQuestionTwo.Note.Id)
            {
                return new List<string> { pairedMainQuestions.MainQuestionOne.Note.Value };
            }

            var notes = new List<string>();

            if (pairedMainQuestions.MainQuestionOne != null && pairedMainQuestions.MainQuestionOne.Note != null)
            {
                notes.Add(pairedMainQuestions.MainQuestionOne.Note.Value);
            }

            if (pairedMainQuestions.MainQuestionTwo != null && pairedMainQuestions.MainQuestionTwo.Note != null)
            {
                notes.Add(pairedMainQuestions.MainQuestionTwo.Note.Value);
            }

            return notes;
        }

        public List<string> GetNotes(IReadOnlyCollection<SubQuestion> subQuestions)
        {
            var notes = new List<Note>();

            foreach (var subQuestion in subQuestions)
            {
                if (subQuestion.Note != null)
                {
                    notes.Add(subQuestion.Note);
                }
            }

            var uniqueNotesAsStrings = notes.GroupBy(x => x.Id)
                .Select(y => y.First())
                .Distinct()
                .Select(x => x.Value);

            return uniqueNotesAsStrings.ToList();
        }

        public string GetNote(SpecialMainQuestion specialMainQuestion)
        {
            if (specialMainQuestion.Note != null)
            {
                return specialMainQuestion.Note.Value;
            }

            return null;
        }

        public List<string> GetNotes(IReadOnlyCollection<SpecialSubQuestion> specialSubQuestions)
        {
            var notes = new List<Note>();

            foreach (var specialSubQuestion in specialSubQuestions)
            {
                if (specialSubQuestion.Note != null)
                {
                    notes.Add(specialSubQuestion.Note);
                }
            }

            var uniqueNotesAsStrings = notes.GroupBy(x => x.Id)
                .Select(y => y.First())
                .Distinct()
                .Select(x => x.Value);

            return uniqueNotesAsStrings.ToList();
        }
    }
}
