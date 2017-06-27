using System.Collections.Generic;
using System.Linq;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModel
    {
        public WorkpackageOneOverviewViewModel WorkpackageOne { get; set; }
        public WorkpackageOneOverviewViewModel WorkpackageTwo { get; set; }
        public WorkpackageOneOverviewViewModel WorkpackageThree { get; set; }

        private IEnumerable<WorkpackageOneOverviewViewModel> Workpackages => new[]
        {
            WorkpackageOne,
            WorkpackageTwo,
            WorkpackageThree
        };

        public void MarkSelectedStep(string stepId)
        {
            var steps = Workpackages.SelectMany(wp => wp?.Steps);

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