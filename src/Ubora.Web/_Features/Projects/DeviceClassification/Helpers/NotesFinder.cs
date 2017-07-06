using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public static class NotesFinder
    {
        public static IReadOnlyCollection<string> GetNotes(this IEnumerable<BaseQuestion> specialSubQuestions)
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
