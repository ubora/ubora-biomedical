using System.Collections.Generic;
using Ubora.Web._Components;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.RelatedProjects.Models
{
    public class RelatedProjectsViewModel
    {
        public IReadOnlyCollection<ProjectCardViewModel> RelatedProjects { get; set; }
    }
}
