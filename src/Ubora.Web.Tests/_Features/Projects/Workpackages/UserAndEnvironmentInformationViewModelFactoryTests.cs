using AutoMapper;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web._Features._Shared;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class UserAndEnvironmentInformationViewModelFactoryTests
    {
        private readonly UserAndEnvironmentInformationViewModel.Factory _factory;

        public UserAndEnvironmentInformationViewModelFactoryTests()
        {
            var autoMapperConfig = new MapperConfiguration(x => x.AddProfile(typeof(AutoMapperProfile)));
            var autoMapper = new Mapper(autoMapperConfig);

            _factory = new UserAndEnvironmentInformationViewModel.Factory(autoMapper);
        }

        [Fact]
        public void Creates_ViewModel_From_Domain_Aggregate()
        {
            var domainAggregate = new UserAndEnvironmentInformation(
                intendedUser: new Other("test222"),
                intendedUserTraining: IntendedUserTraining.CreateTrainingRequired("test111"), 
                isAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse: true,
                whereWillTechnologyBeUsed: new WhereWillTechnologyBeUsed(
                    atHome: true,
                    indoors: true,
                    outdoors: true,
                    tertiaryLevel: false,
                    primaryLevel: true,
                    ruralSettings: true,
                    secondaryLevel: true,
                    urbanSettings: true
                ));

            // Act
            var createdModel = _factory.Create(domainAggregate);

            // Assert
            var expectedModel = new UserAndEnvironmentInformationViewModel
            {
                IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining = "test111",
                IntendedUserIfOther = "test222",
                IntendedUserTypeKey = "other",
                IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse = true,
                IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser = true,
                WhereWillTechnologyBeUsed = new WhereWillTechnologyBeUsedViewModel
                {
                    AtHome = true,
                    Indoors = true,
                    Outdoors = true,
                    TertiaryLevel = false,
                    PrimaryLevel = true,
                    RuralSettings = true,
                    SecondaryLevel = true,
                    UrbanSettings = true
                }
            };

            createdModel.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
