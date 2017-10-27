using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class ConceptualDesignViewModel
    {
        public IEnumerable<CandidateItemViewModel> Candidates { get; set; }
    }

    public class CandidateItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
