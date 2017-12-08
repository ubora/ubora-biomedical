using System;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Portabilities;
using Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance;
using Ubora.Domain.Projects.StructuredInformations.TypesOfUse;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class HealthTechnologySpecificationsViewModelMapperTests
    {
        private readonly HealthTechnologySpecificationsViewModel.Mapper _mapper;

        public HealthTechnologySpecificationsViewModelMapperTests()
        {
            _mapper = new HealthTechnologySpecificationsViewModel.Mapper();
        }

        [Fact]
        public void Maps_DeviceMeasurements()
        {
            var lenght = 100;
            var height = 200;
            var width = 300;
            var weight = 400;

            var model = new HealthTechnologySpecificationsViewModel
            {
                DeviceMeasurementsViewModel = new DeviceMeasurementsViewModel
                {
                    DimensionsLength = lenght,
                    DimensionsHeight = height,
                    DimensionsWidth = width,
                    WeightInKilograms = weight
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.DeviceMeasurements, new DeviceMeasurements(height, lenght, width, weight));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_UseOfConsumables_If_Required()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                DoesItRequireUseOfConsumables = true,
                IfRequiresConsumablesListConsumables = "test"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.UseOfConsumables, UseOfConsumables.CreateUseOfConsumablesIfRequired(consumables: "test"));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_UseOfConsumables_If_Not_Required()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                DoesItRequireUseOfConsumables = false,
                IfRequiresConsumablesListConsumables = "test"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.UseOfConsumables, UseOfConsumables.CreateUseOfConsumablesIfNotRequired());

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_EstimatedLifeTime()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                EstimatedLifeTimeInDays = 1,
                EstimatedLifeTimeInMonths = 2,
                EstimatedLifeTimeInYears = 3
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.EstimatedLifeTime, new Duration(days: 1, months: 2, years: 3));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_EstimatedShelfTime()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                EstimatedShelfLifeInDays = 1,
                EstimatedShelfLifeInMonths = 2,
                EstimatedShelfLifeInYears = 3
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.EstimatedShelfTime, new Duration(days: 1, months: 2, years: 3));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_CanItHaveATelemedicineOrEHealthApplication()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                CanItHaveATelemedicineOrEHealthApplication = true
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.CanItHaveATelemedicineOrEHealthApplication, true);

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_DeviceSoftwareUsage_If_UsesSoftware()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                DeviceSoftwareUsageViewModel = new DeviceSoftwareUsageViewModel
                {
                    DoesItUseAnyKindOfSoftware = true,
                    IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = "test111",
                    IfUsesSoftwareDescribeSoftware = "test222"
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.DeviceSoftwareUsage, DeviceSoftwareUsage.CreateSoftwareIsUsed(description: "test222", localUseDescription: "test111"));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_DeviceSoftwareUsage_If_DoesNotUseSoftware()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                DeviceSoftwareUsageViewModel = new DeviceSoftwareUsageViewModel
                {
                    DoesItUseAnyKindOfSoftware = false,
                    IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = "test111",
                    IfUsesSoftwareDescribeSoftware = "test222"
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.DeviceSoftwareUsage, DeviceSoftwareUsage.CreateSoftwareIsNotUsed());

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_Portability()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                PortabilityKey = "mobile"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.Portability, new Mobile());

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_TypeOfUse()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                TypeOfUseKey = "long_term_use"
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.TypeOfUse, new LongTermUse());

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_MaintenanceInformation_If_Maintenance_Required()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                TechnologyMaintenanceViewModel = new TechnologyMaintenanceViewModel
                {
                    DoesTechnologyRequireMaintenance = true,
                    IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity = true,
                    IfTechnologyRequiresMaintenanceSpecifyFrequency = "frequency",
                    IfTechnologyRequiresMaintenanceSpecifyType = "type",
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance = "other",
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther = "provider"
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.Maintenance, MaintenanceInformation.CreateMaintenanceRequired(
                    type: "type",
                    frequency: "frequency",
                    canBeDoneOnSitrOrHomeOrCommunity: true,
                    provider: new OtherProviderOfMaintenance("provider")
                    ));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_MaintenanceInformation_If_Maintenance_Not_Required()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                TechnologyMaintenanceViewModel = new TechnologyMaintenanceViewModel
                {
                    DoesTechnologyRequireMaintenance = false,
                    IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity = true,
                    IfTechnologyRequiresMaintenanceSpecifyFrequency = "frequency",
                    IfTechnologyRequiresMaintenanceSpecifyType = "type",
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance = "other",
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther = "provider"
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.Maintenance, MaintenanceInformation.CreateMaintenanceNotRequired());

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_EnergyRequirementsInformation()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                EnergyRequirements = new EnergyRequirementsViewModel
                {
                    MechanicalEnergy = true,
                    Batteries = true,
                    PowerSupplyForRecharging = true,
                    IfPowerSupplyForRechargingThenRequiredVoltage = 10,
                    IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours = 2,
                    IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes = 3,
                    IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours = 4,
                    IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes = 5,
                    ContinuousPowerSupply = true,
                    IfContinuousPowerSupplyThenRequiredVoltage = 20,
                    SolarPower = true,
                    IfSolarPowerThenTimeInSunlightRequiredToChargeInHours = 6,
                    IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes = 7,
                    IfSolarPowerThenBatteryLifeInHours = 8,
                    IfSolarPowerThenBatteryLifeInMinutes = 9,
                    Other = true,
                    OtherText = "otherText"
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var powerSupplyForCharging = PowerSupplyForRecharging.CreatePowerSupplyForRechargingRequired(
                voltage: 10,
                timeToRecharge: new TimeSpan(hours: 2, minutes: 3, seconds: 0),
                batteryLife: new TimeSpan(hours: 4, minutes: 5, seconds: 0)
            );

            var continuousPowerSupply = ContinuousPowerSupply.CreateContinuousPowerSupplyRequired(voltage: 20);

            var solarPower = SolarPower.CreateSolarPowerRequired(
                timeToCharge: new TimeSpan(hours: 6, minutes: 7, seconds: 0),
                batteryLife: new TimeSpan(hours: 8, minutes: 9, seconds: 0)
            );

            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.EnergyRequirements, new EnergyRequirementsInformation(
                    mechanicalEnergy: true,
                    batteries: true,
                    powerSupplyForRecharging: powerSupplyForCharging,
                    continuousPowerSupply: continuousPowerSupply,
                    solarPower: solarPower,
                    other: true,
                    otherText: "otherText"
                    ));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Maps_FacilityRequirements()
        {
            var model = new HealthTechnologySpecificationsViewModel
            {
                FacilityRequirements = new FacilityRequirementsViewModel
                {
                    CleanWaterSupply = true,
                    SpecificTemperatureAndOrHumidityRange = true,
                    IfSpecificTemperatureAndOrHumidityRangeThenDescription = "test111",
                    ClinicalWasteDisposalFacilities = true,
                    IfClinicalWasteDisposalFacilitiesThenDescription = "test222",
                    GasSupply = true,
                    IfGasSupplyThenDescription = "test333",
                    Sterilization = true,
                    IfSterilizationThenDescription = "test444",
                    RadiationIsolation = true,
                    AccessToCellularPhoneNetwork = true,
                    AccessToInternet = true,
                    AccessibleByCar = true,
                    AdditionalSoundOrLightControlFacilites = true,
                    ConnectionToLaptopComputer = true,
                    Other = true,
                    OtherText = "other"
                }
            };

            // Act
            var mappedCommand = _mapper.MapToCommand(model);

            // Assert
            var expected = HealthTechnologySpecificationsInformation.CreateEmpty()
                .Set(x => x.FacilityRequirements, new FacilityRequirementsInformation(
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
                    otherText: "other"
                    ));

            mappedCommand.HealthTechnologySpecificationsInformation.ShouldBeEquivalentTo(expected);
        }

    }
}