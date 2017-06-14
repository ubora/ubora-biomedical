using System.Collections.Generic;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModel
    {
        public IEnumerable<WorkpackageOneOverviewViewModel> Workpackages { get; set; }
    }
}