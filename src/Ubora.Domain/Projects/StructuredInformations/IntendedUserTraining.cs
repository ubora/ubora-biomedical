using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class IntendedUserTraining
    {
        [JsonConstructor]
        private IntendedUserTraining()
        {
        }

        public bool IsTrainingRequiredInAdditionToExpectedSkillLevel { get; private set; }
        public string DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequired { get; private set; }

        public static IntendedUserTraining CreateTrainingRequired(string descriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequired)
        {
            return new IntendedUserTraining
            {
                IsTrainingRequiredInAdditionToExpectedSkillLevel = true,
                DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequired = descriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequired
            };
        }

        public static IntendedUserTraining CreateTrainingNotRequired()
        {
            return new IntendedUserTraining
            {
                IsTrainingRequiredInAdditionToExpectedSkillLevel = false
            };
        }

        public static IntendedUserTraining CreateEmpty()
        {
            return new IntendedUserTraining();
        }
    }
}