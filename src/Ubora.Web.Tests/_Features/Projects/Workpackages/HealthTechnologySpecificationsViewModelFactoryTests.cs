using AutoMapper;
using FluentAssertions;
using Moq;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Portabilities;
using Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance;
using Ubora.Domain.Projects.StructuredInformations.TypesOfUse;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class HealthTechnologySpecificationsViewModelFactoryTests
    {
        private readonly HealthTechnologySpecificationsViewModel.Factory _factory;
        private readonly Mock<IMapper> _autoMapperMock;

        public HealthTechnologySpecificationsViewModelFactoryTests()
        {
            _autoMapperMock = new Mock<IMapper>();
            _factory = new HealthTechnologySpecificationsViewModel.Factory(_autoMapperMock.Object);
        }

        [Fact]
        public void Creates_ViewModel_From_Domain_Aggregate()
        {
            var facilityRequirementsInformation = new FacilityRequirementsInformation(
                specificTemperatureAndOrHumidityRange: true,
                ifSpecificTemperatureAndOrHumidityRangeThenDescription: "test111",
                clinicalWasteDisposalFacilities: true,
                ifClinicalWasteDisposalFacilitiesThenDescription: "test222",
                gasSupply: true,
                ifGasSupplyThenDescription: "test333",
                sterilization: true,
                ifSterilizationThenDescription: "test444",
                radiationIsolation: true,
                cleanWaterSupply: true,
                accessToInternet: true,
                accessToCellularPhoneNetwork: true,
                connectionToLaptopComputer: true,
                accessibleByCar: true,
                additionalSoundOrLightControlFacilites: true,
                other: true,
                otherText: "otherText"
            );

            var domainAggregate = new HealthTechnologySpecificationsInformation(
                deviceMeasurements: new DeviceMeasurements(
                    dimensionsHeight: 1,
                    dimensionsLength: 2,
                    dimensionsWidth: 3,
                    weightInKilograms: 4
                    ), 
                useOfConsumables: UseOfConsumables.CreateUseOfConsumablesIfRequired(consumables: "consumables"),
                estimatedLifeTime: new Duration(1,2,3),
                estimatedShelfTime: new Duration(3,2,1),
                canItHaveATelemedicineOrEHealthApplication: true,
                deviceSoftwareUsage: DeviceSoftwareUsage.CreateSoftwareIsUsed(
                    description: "description",
                    localUseDescription: "localUseDescription"
                    ),
                portability: new Mobile(), 
                typeOfUse: new SingleUse(), 
                maintenance: MaintenanceInformation.CreateMaintenanceRequired(
                    type: "type",
                    frequency: "frequency",
                    canBeDoneOnSitrOrHomeOrCommunity: true,
                    provider: new OtherProviderOfMaintenance("otherProvider")
                    ), 
                energyRequirements: new EnergyRequirementsInformation(
                    mechanicalEnergy: true,
                    batteries: true,
                    powerSupplyForRecharging: PowerSupplyForRecharging.CreatePowerSupplyForRechargingNotRequired(), 
                    continuousPowerSupply: ContinuousPowerSupply.CreateContinuousPowerSupplyNotRequired(), 
                    solarPower: SolarPower.CreateSolarPowerNotRequired(),
                    other: true,
                    otherText: "otherEnergyRequirement"
                    ), 
                facilityRequirements: facilityRequirementsInformation
                );

            var expectedFacilityRequirementsViewModel = new FacilityRequirementsViewModel
            {
                AccessToCellularPhoneNetwork = true,
                AccessibleByCar = true,
                AccessToInternet = true,
                AdditionalSoundOrLightControlFacilites = true,
                CleanWaterSupply = true,
                ClinicalWasteDisposalFacilities = true,
                IfClinicalWasteDisposalFacilitiesThenDescription = "test222",
                Sterilization = true,
                IfSterilizationThenDescription = "test444",
                GasSupply = true,
                IfGasSupplyThenDescription = "test333",
                SpecificTemperatureAndOrHumidityRange = true,
                IfSpecificTemperatureAndOrHumidityRangeThenDescription = "test111",
                RadiationIsolation = true,
                ConnectionToLaptopComputer = true,
                Other = true,
                OtherText = "otherText"
            };

            _autoMapperMock.Setup(x => x.Map<FacilityRequirementsViewModel>(facilityRequirementsInformation))
                .Returns(expectedFacilityRequirementsViewModel);

            // Act
            var createdModel = _factory.Create(domainAggregate);

            var expectedModel = new HealthTechnologySpecificationsViewModel
            {
                CanItHaveATelemedicineOrEHealthApplication = true,
                DeviceMeasurementsViewModel = new DeviceMeasurementsViewModel
                {
                    DimensionsHeight = 1,
                    DimensionsLength = 2,
                    DimensionsWidth = 3,
                    WeightInKilograms = 4
                },
                DeviceSoftwareUsageViewModel = new DeviceSoftwareUsageViewModel
                {
                    DoesItUseAnyKindOfSoftware = true,
                    IfUsesSoftwareDescribeSoftware = "description",
                    IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = "localUseDescription"
                },
                DoesItRequireUseOfConsumables = true,
                IfRequiresConsumablesListConsumables = "consumables",
                EstimatedLifeTimeInDays = 1,
                EstimatedLifeTimeInMonths = 2,
                EstimatedLifeTimeInYears = 3,
                EstimatedShelfLifeInDays = 3,
                EstimatedShelfLifeInMonths = 2,
                EstimatedShelfLifeInYears = 1,
                PortabilityKey = "mobile",
                TypeOfUseKey = "single_use",
                TechnologyMaintenanceViewModel = new TechnologyMaintenanceViewModel
                {
                    DoesTechnologyRequireMaintenance = true,
                    IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity = true,
                    IfTechnologyRequiresMaintenanceSpecifyType = "type",
                    IfTechnologyRequiresMaintenanceSpecifyFrequency = "frequency",
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance = "other",
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther = "otherProvider"
                },
                EnergyRequirements = new EnergyRequirementsViewModel
                {
                    Batteries = true,
                    ContinuousPowerSupply = false,
                    PowerSupplyForRecharging = false,
                    SolarPower = false,
                    MechanicalEnergy = true,
                    Other = true,
                    OtherText = "otherEnergyRequirement",
                },
                FacilityRequirements = expectedFacilityRequirementsViewModel
            };

            createdModel.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
