using System;
using Newtonsoft.Json;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class UserAndEnvironmentInformation
    {
        public UserAndEnvironmentInformation(
            IntendedUser intendedUser,
            IntendedUserTraining intendedUserTraining,
            bool isAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse,
            WhereWillTechnologyBeUsed whereWillTechnologyBeUsed)
        {
            IntendedUser = intendedUser;
            IntendedUserTraining = intendedUserTraining ?? throw new ArgumentNullException(nameof(intendedUserTraining));
            IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse = isAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse;
            WhereWillTechnologyBeUsed = whereWillTechnologyBeUsed ?? throw new ArgumentNullException(nameof(whereWillTechnologyBeUsed));
        }

        [JsonConstructor]
        protected UserAndEnvironmentInformation()
        {
        }

        public IntendedUser IntendedUser { get; private set; }
        public IntendedUserTraining IntendedUserTraining { get; private set; } = IntendedUserTraining.CreateEmpty();
        public bool IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse { get; private set; }
        public WhereWillTechnologyBeUsed WhereWillTechnologyBeUsed { get; private set; } = WhereWillTechnologyBeUsed.CreateEmpty();

        public static UserAndEnvironmentInformation CreateEmpty()
        {
            return new UserAndEnvironmentInformation();
        }
    }
}