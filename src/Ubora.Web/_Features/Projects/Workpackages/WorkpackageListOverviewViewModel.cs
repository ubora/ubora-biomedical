using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModel
    {
        public IEnumerable<WorkpackageOneOverviewViewModel> Workpackages { get; set; }

        public void MarkSelectedStep(Guid taskId)
        {
            var steps = Workpackages.SelectMany(wp => wp.Steps);

            foreach (var step in steps)
            {
                if (step.Id == taskId)
                {
                    step.IsSelected = true;
                    break;
                }
            }
        }
    }
}