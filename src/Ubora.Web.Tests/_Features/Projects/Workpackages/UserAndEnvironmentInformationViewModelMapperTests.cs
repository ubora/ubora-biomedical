using System;
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
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = UserAndEnvironmentInformation.CreateEmpty()
                .Set(x => x.IntendedUser, new Nurse());

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
            var expected = UserAndEnvironmentInformation.CreateEmpty()
                .Set(x => x.IntendedUser, new Other("testOther"));

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_WhereWillTechnologyBeUsed()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
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
            var whereWillTechnologyBeUsedInformation = new WhereWillTechnologyBeUsed
            (
                atHome: true,
                indoors: true,
                outdoors: true,
                tertiaryLevel: true,
                primaryLevel: true,
                ruralSettings: true,
                secondaryLevel: true,
                urbanSettings: true
            );

            var expected = UserAndEnvironmentInformation.CreateEmpty()
                .Set(x => x.WhereWillTechnologyBeUsed, whereWillTechnologyBeUsedInformation);

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining_If_Yes()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = true,
                IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining = "test"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = UserAndEnvironmentInformation.CreateEmpty()
                .Set(x => x.IntendedUserTraining, IntendedUserTraining.CreateTrainingRequired("test"));

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Does_Not_Map_IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining_If_No()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = false,
                IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining = "test"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = UserAndEnvironmentInformation.CreateEmpty();

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse()
        {
            var model = new UserAndEnvironmentInformationViewModel
            {
                IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse = true
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = UserAndEnvironmentInformation.CreateEmpty()
                .Set(x => x.IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse, true);

            mappedCommand.UserAndEnvironmentInformation.ShouldBeEquivalentTo(expected);
        }

    }
}
