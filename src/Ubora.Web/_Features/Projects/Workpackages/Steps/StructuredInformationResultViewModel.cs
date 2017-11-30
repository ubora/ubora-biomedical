using Ubora.Domain.Projects.StructuredInformations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class StructuredInformationResultViewModel
    {
        public UserAndEnvironmentResult UserAndEnvironment { get; set; }

        public class Factory
        {
            private readonly UserAndEnvironmentResult.Factory _userAndEnvironmentFactory;

            public Factory(UserAndEnvironmentResult.Factory userAndEnvironmentFactory)
            {
                _userAndEnvironmentFactory = userAndEnvironmentFactory;
            }

            public StructuredInformationResultViewModel Create(DeviceStructuredInformation deviceStructuredInformation)
            {
                return new StructuredInformationResultViewModel
                {
                    UserAndEnvironment = _userAndEnvironmentFactory.Create(deviceStructuredInformation.UserAndEnvironment)
                };
            }
        }
    }
}