using Ubora.Domain.Projects.StructuredInformations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class StructuredInformationResultViewModel
    {
        public UserAndEnvironmentResult UserAndEnvironment { get; set; }
        public HealthTechnologySpecificationsResult HealthTechnologySpecifications { get; set; }
        public bool IsUserAndEnvironmentEdited { get; set; }
        public bool IsHealthTechnologySpecificationEdited { get; set; }

        public class Factory
        {
            private readonly UserAndEnvironmentResult.Factory _userAndEnvironmentFactory;
            private readonly HealthTechnologySpecificationsResult.Factory _healthTechnologySpecifications;

            public Factory(UserAndEnvironmentResult.Factory userAndEnvironmentFactory, HealthTechnologySpecificationsResult.Factory healthTechnologySpecifications)
            {
                _userAndEnvironmentFactory = userAndEnvironmentFactory;
                _healthTechnologySpecifications = healthTechnologySpecifications;
            }

            public StructuredInformationResultViewModel Create(DeviceStructuredInformation deviceStructuredInformation)
            {
                return new StructuredInformationResultViewModel
                {
                    UserAndEnvironment = _userAndEnvironmentFactory.Create(deviceStructuredInformation.UserAndEnvironment),
                    HealthTechnologySpecifications = _healthTechnologySpecifications.Create(deviceStructuredInformation.HealthTechnologySpecification),
                    IsUserAndEnvironmentEdited = deviceStructuredInformation.IsUserAndEnvironmentEdited,
                    IsHealthTechnologySpecificationEdited = deviceStructuredInformation.IsHealthTechnologySpecificationEdited
                };
            }
        }
    }
}