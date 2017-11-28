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
                IntendedUserTypeKey = "nurse",
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel()
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Nurse(),
                WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation()
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
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel()
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Other("testOther"),
                WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation()
            };
            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_WhereWillTechnologyBeUsed()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IntendedUserTypeKey = "nurse",
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel
                {
                    AtHome = true,
                    Indoors = true,
                    Outdoors = true,
                    TertiaryLevel = true,
                    PrimaryLevel = true,
                    RuralSettings = true,
                    SecondaryLevel = true,
                    UrbanSettings = true
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Nurse(),
                WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation
                {
                    AtHome = true,
                    Indoors = true,
                    Outdoors = true,
                    TertiaryLevel = true,
                    PrimaryLevel = true,
                    RuralSettings = true,
                    SecondaryLevel = true,
                    UrbanSettings = true
                }
            };

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining_If_Yes()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IntendedUserTypeKey = "nurse",
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel(),
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = true,
                IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining = "test"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Nurse(),
                WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation(),
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = true,
                DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTrainingIntendedUser = "test"
            };

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Does_Not_Map_IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining_If_No()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IntendedUserTypeKey = "nurse",
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel(),
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = false,
                IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining = "test"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Nurse(),
                WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation(),
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = false,
            };

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IntendedUserTypeKey = "nurse",
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel(),
                IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse = true
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = new DeviceStructuredInformation.UserAndEnvironmentInformation
            {
                IntendedUser = new Nurse(),
                WhereWillTechnologyBeUsed = new DeviceStructuredInformation.UserAndEnvironmentInformation.WhereWillTechnologyBeUsedInformation(),
                IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse = true
            };

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

    }
}
