using System.Collections.Generic;
using System.Linq;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModel
    {
        public IEnumerable<WorkpackageOneOverviewViewModel> Workpackages { get; set; }

        public void MarkSelectedStep(string stepId)
        {
            var steps = Workpackages.SelectMany(wp => wp.Steps);

            foreach (var step in steps)
            {
                if (step.Id == stepId)
                {
                    step.IsSelected = true;
                    break;
                }
            }
        }
    }
}