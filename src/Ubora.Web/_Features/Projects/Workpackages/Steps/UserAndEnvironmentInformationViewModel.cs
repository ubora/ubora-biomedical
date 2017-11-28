using System;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class UserAndEnvironmentInformationViewModel
    {
        public string IntendedUserTypeKey { get; set; }
        public string IntendedUserIfOther { get; set; }
        public bool IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser { get; set; }
        public string IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining { get; set; }
        public bool IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse { get; set; }
        public WhereWillTechnologyBeUsedViewModel WhereWillTechnologyBeUsed { get; set; }

        public class Mapper
        {
            public virtual EditDeviceStructuredInformationOnUserAndEnvironmentCommand MapToCommand(UserAndEnvironmentInformationViewModel model)
            {
                var userAndEnvironmentInformation = new DeviceStructuredInformation.UserAndEnvironmentInformation();

                userAndEnvironmentInformation.IntendedUser = MapIntendedUser(model);

                userAndEnvironmentInformation.IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = model.IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser;
                if (model.IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser)
                {
                    userAndEnvironmentInformation.DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTrainingIntendedUser = model.IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining;
                }

                userAndEnvironmentInformation.IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse = model.IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse;

                var whereWillTechnologyBeUsedModel = model.WhereWillTechnologyBeUsed;
                userAndEnvironmentInformation.WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation
                {
                    TertiaryLevel = whereWillTechnologyBeUsedModel.TertiaryLevel,
                    SecondaryLevel = whereWillTechnologyBeUsedModel.SecondaryLevel,
                    UrbanSettings = whereWillTechnologyBeUsedModel.UrbanSettings,
                    Indoors = whereWillTechnologyBeUsedModel.Indoors,
                    RuralSettings = whereWillTechnologyBeUsedModel.RuralSettings,
                    Outdoors = whereWillTechnologyBeUsedModel.Outdoors,
                    PrimaryLevel = whereWillTechnologyBeUsedModel.PrimaryLevel,
                    AtHome = whereWillTechnologyBeUsedModel.AtHome
                };

                var command = new EditDeviceStructuredInformationOnUserAndEnvironmentCommand
                {
                    UserAndEnvironmentInformation = userAndEnvironmentInformation
                };
                return command;
            }

            private IntendedUser MapIntendedUser(UserAndEnvironmentInformationViewModel model)
            {
                var isIntendedUserSpecified = IntendedUser.IntendedUserKeyTypeMap.ContainsKey(model.IntendedUserTypeKey);
                if (isIntendedUserSpecified)
                {
                    var intendedUserType = IntendedUser.IntendedUserKeyTypeMap[model.IntendedUserTypeKey];
                    if (intendedUserType == typeof(Other))
                    {
                        return new Other(model.IntendedUserIfOther);
                    }
                    else
                    {
                        return (IntendedUser) Activator.CreateInstance(type: intendedUserType);
                    }
                }
                else
                {
                    return null;
                }
            }
        }
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
