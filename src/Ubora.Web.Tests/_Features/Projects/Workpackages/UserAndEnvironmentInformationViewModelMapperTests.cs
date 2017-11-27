using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class UserAndEnvironmentInformationViewModelMapperTests
    {
        private readonly UserAndEnvironmentInformationViewModel.Mapper _mapper;

        public UserAndEnvironmentInformationViewModelMapperTests()
        {
            _mapper = new UserAndEnvironmentInformationViewModel.Mapper();
        }

        [Fact]
        public void Maps_Preknown_Intended_Users()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IntendedUserTypeKey = "nurse"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Nurse()
            };

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_Other_Intended_Users()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IntendedUserTypeKey = "other",
                IntendedUserIfOther = "testOther",
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Other("testOther")
            };
            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }
    }
}
