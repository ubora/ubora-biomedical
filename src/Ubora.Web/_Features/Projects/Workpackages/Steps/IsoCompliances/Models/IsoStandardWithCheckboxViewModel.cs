namespace Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models
{
    public class IsoStandardWithCheckboxViewModel
    {
        public IsoStandardViewModel IsoStandard { get; set; }
        public bool CanEditIsoStandard { get; set; }
        public bool CanRemoveIsoStandardFromComplianceChecklist { get; set; }
    }
}
