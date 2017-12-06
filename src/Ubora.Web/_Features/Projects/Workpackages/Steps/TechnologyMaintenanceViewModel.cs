using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class TechnologyMaintenanceViewModel
    {
        [Required(ErrorMessage="The field is required.")]
        public bool DoesTechnologyRequireMaintenance { get; set; }
        [RequiredIf(nameof(DoesTechnologyRequireMaintenance), true)]
        public string IfTechnologyRequiresMaintenanceSpecifyType { get; set; }
        [RequiredIf(nameof(DoesTechnologyRequireMaintenance), true)]
        public string IfTechnologyRequiresMaintenanceSpecifyFrequency { get; set; }
        [RequiredIf(nameof(DoesTechnologyRequireMaintenance), true)]
        public bool IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity { get; set; }
        [RequiredIf(nameof(DoesTechnologyRequireMaintenance), true)]
        public string IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance { get; set; }
        [RequiredIf(nameof(DoesTechnologyRequireMaintenance), true)]
        [RequiredIf(nameof(IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance), "other")]
        public string IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther { get; set; }
    }
}