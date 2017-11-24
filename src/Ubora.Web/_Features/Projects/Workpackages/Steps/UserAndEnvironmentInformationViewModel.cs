using System.ComponentModel.DataAnnotations;
using Ubora.Domain.Projects.StructuredInformations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class UserAndEnvironmentInformationViewModel
    {
        [Required(ErrorMessage = "Please answer this question!")]
        public IntendedUser IntendedUser { get; set; }
        public string IntendedUserOther { get; set; }
        [Required(ErrorMessage = "Please answer this question!")]
        public bool IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser { get; set; }
        public string IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining { get; set; }
        [Required(ErrorMessage = "Please answer this question!")]
        public bool IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse { get; set; }
        [Required(ErrorMessage = "Please answer this question!")]
        public WhereWillTechnologyBeUsedViewModel WhereWillTechnologyBeUsed { get; set; }
    }

    public class WhereWillTechnologyBeUsedViewModel
    {
        public bool RuralSettings { get; set; }
        public bool UrbanSettings { get; set; }
        public bool Outdoors { get; set; }
        public bool Indoors { get; set; }
        public bool AtHome { get; set; }
        public bool PrimaryLevel { get; set; }
        public bool SecondaryLevel { get; set; }
        public bool TertiaryLevel { get; set; }
    }
}
