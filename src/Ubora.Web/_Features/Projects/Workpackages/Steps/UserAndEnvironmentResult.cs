using AutoMapper;
using Ubora.Domain.Projects.StructuredInformations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class UserAndEnvironmentResult
    {
        public string IntendedUser { get; set; }
        public bool IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser { get; set; }
        public string IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining { get; set; }
        public bool IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse { get; set; }
        public WhereWillTechnologyBeUsedViewModel WhereWillTechnologyBeUsed { get; set; }

        public class Factory
        {
            private readonly IMapper _autoMapper;

            public Factory(IMapper autoMapper)
            {
                _autoMapper = autoMapper;
            }

            public UserAndEnvironmentResult Create(UserAndEnvironmentInformation userAndEnvironment)
            {
                return new UserAndEnvironmentResult
                {
                    IntendedUser = userAndEnvironment.IntendedUser?.ToDisplayName(),
                    IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = userAndEnvironment.IntendedUserTraining.IsTrainingRequiredInAdditionToExpectedSkillLevel,
                    IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining = userAndEnvironment.IntendedUserTraining.DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequired,
                    IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse = userAndEnvironment.IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse,
                    WhereWillTechnologyBeUsed = _autoMapper.Map<WhereWillTechnologyBeUsedViewModel>(userAndEnvironment.WhereWillTechnologyBeUsed)
                };
            }
        }
    }
}