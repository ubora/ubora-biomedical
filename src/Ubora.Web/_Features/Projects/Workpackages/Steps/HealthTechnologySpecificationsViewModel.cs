using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Ubora.Domain.Projects.StructuredInformations.Portabilities;
using Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance;
using Ubora.Domain.Projects.StructuredInformations.TypesOfUse;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{

    public class HealthTechnologySpecificationsViewModel
    {
        public DeviceMeasurementsViewModel DeviceMeasurementsViewModel { get; set; } = new DeviceMeasurementsViewModel();
        [Required]
        public bool DoesItRequireUseOfConsumables { get; set; }
        [RequiredIf(nameof(DoesItRequireUseOfConsumables), true)]
        public string IfRequiresConsumablesListConsumables { get; set; }
        [Required]
        public int EstimatedLifeTimeInDays { get; set; }
        [Required]
        public int EstimatedLifeTimeInMonths { get; set; }
        [Required]
        public int EstimatedLifeTimeInYears { get; set; }
        [Required]
        public int EstimatedShelfLifeInDays { get; set; }
        [Required]
        public int EstimatedShelfLifeInMonths { get; set; }
        [Required]
        public int EstimatedShelfLifeInYears { get; set; }
        [Required]
        public bool CanItHaveATelemedicineOrEHealthApplication { get; set; }
        public DeviceSoftwareUsageViewModel DeviceSoftwareUsageViewModel { get; set; } = new DeviceSoftwareUsageViewModel();
        [Required]
        public string PortabilityKey { get; set; }
        [Required]
        public string TypeOfUseKey { get; set; }
        public TechnologyMaintenanceViewModel TechnologyMaintenanceViewModel { get; set; } = new TechnologyMaintenanceViewModel();
        public EnergyRequirementsViewModel EnergyRequirements { get; set; } = new EnergyRequirementsViewModel();
        public FacilityRequirementsViewModel FacilityRequirements { get; set; } = new FacilityRequirementsViewModel();

        public class Factory
        {
            private readonly IMapper _autoMapper;

            public Factory(IMapper autoMapper)
            {
                _autoMapper = autoMapper;
            }

            protected Factory()
            {
            }

            public virtual HealthTechnologySpecificationsViewModel Create(HealthTechnologySpecificationsInformation domainAggregate)
            {
                var model = new HealthTechnologySpecificationsViewModel();

                model.DeviceMeasurementsViewModel = new DeviceMeasurementsViewModel
                {
                    DimensionsWidth = domainAggregate.DeviceMeasurements.DimensionsWidth,
                    DimensionsHeight = domainAggregate.DeviceMeasurements.DimensionsHeight,
                    DimensionsLength = domainAggregate.DeviceMeasurements.DimensionsLength,
                    WeightInKilograms = domainAggregate.DeviceMeasurements.WeightInKilograms
                };

                model.DoesItRequireUseOfConsumables = domainAggregate.UseOfConsumables.IsRequired;
                model.IfRequiresConsumablesListConsumables = domainAggregate.UseOfConsumables.IfRequiresConsumablesListConsumables;

                model.EstimatedLifeTimeInDays = domainAggregate.EstimatedLifeTime.Days;
                model.EstimatedLifeTimeInMonths = domainAggregate.EstimatedLifeTime.Months;
                model.EstimatedLifeTimeInYears = domainAggregate.EstimatedLifeTime.Years;

                model.EstimatedShelfLifeInDays = domainAggregate.EstimatedShelfTime.Days;
                model.EstimatedShelfLifeInMonths = domainAggregate.EstimatedShelfTime.Months;
                model.EstimatedShelfLifeInYears = domainAggregate.EstimatedShelfTime.Years;

                model.CanItHaveATelemedicineOrEHealthApplication = domainAggregate.CanItHaveATelemedicineOrEHealthApplication;

                model.DeviceSoftwareUsageViewModel = new DeviceSoftwareUsageViewModel
                {
                    DoesItUseAnyKindOfSoftware = domainAggregate.DeviceSoftwareUsage.DoesItUseAnyKindOfSoftware,
                    IfUsesSoftwareDescribeSoftware = domainAggregate.DeviceSoftwareUsage.IfUsesSoftwareDescribeSoftware,
                    IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = domainAggregate.DeviceSoftwareUsage.IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse
                };

                model.PortabilityKey = domainAggregate.Portability.Key;

                model.TypeOfUseKey = domainAggregate.TypeOfUse.Key;

                model.TechnologyMaintenanceViewModel = new TechnologyMaintenanceViewModel
                {
                    DoesTechnologyRequireMaintenance = domainAggregate.Maintenance.DoesTechnologyRequireMaintenance,
                    IfTechnologyRequiresMaintenanceSpecifyType = domainAggregate.Maintenance.MaintenanceType,
                    IfTechnologyRequiresMaintenanceSpecifyFrequency = domainAggregate.Maintenance.MaintenanceFrequency,
                    IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity = domainAggregate.Maintenance.CanMaintenanceBeDoneOnSiteOrHomeOrCommunity,
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance = domainAggregate.Maintenance.ProviderOfMaintenance.Key,
                };
                if (domainAggregate.Maintenance.ProviderOfMaintenance.GetType() == typeof(OtherProviderOfMaintenance))
                {
                    model.TechnologyMaintenanceViewModel.IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther = domainAggregate.Maintenance.ProviderOfMaintenance.ToDisplayName();
                }

                model.EnergyRequirements = new EnergyRequirementsViewModel
                {
                    Batteries = domainAggregate.EnergyRequirements.Batteries,
                    ContinuousPowerSupply = domainAggregate.EnergyRequirements.ContinuousPowerSupply.IsRequired,
                    IfContinuousPowerSupplyThenRequiredVoltage = domainAggregate.EnergyRequirements.ContinuousPowerSupply.IfContinuousPowerSupplyThenRequiredVoltage,
                    PowerSupplyForRecharging = domainAggregate.EnergyRequirements.PowerSupplyForRecharging.IsRequired,
                    IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours = domainAggregate.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredBatteryLife.Hours,
                    IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes = domainAggregate.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredBatteryLife.Minutes,
                    IfPowerSupplyForRechargingThenRequiredVoltage = domainAggregate.EnergyRequirements.PowerSupplyForRecharging.IfPowerSupplyForRechargingThenRequiredVoltage,
                    IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours = domainAggregate.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredTimeToRecharge.Hours,
                    IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes = domainAggregate.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredTimeToRecharge.Minutes,
                    SolarPower = domainAggregate.EnergyRequirements.SolarPower.IsRequired,
                    IfSolarPowerThenBatteryLifeInHours = domainAggregate.EnergyRequirements.SolarPower.SolarPowerBatteryLife.Hours,
                    IfSolarPowerThenBatteryLifeInMinutes = domainAggregate.EnergyRequirements.SolarPower.SolarPowerBatteryLife.Minutes,
                    IfSolarPowerThenTimeInSunlightRequiredToChargeInHours = domainAggregate.EnergyRequirements.SolarPower.SolarPowerTimeInSunlightRequiredToCharge.Hours,
                    IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes = domainAggregate.EnergyRequirements.SolarPower.SolarPowerTimeInSunlightRequiredToCharge.Minutes,
                    MechanicalEnergy = domainAggregate.EnergyRequirements.MechanicalEnergy,
                    Other = domainAggregate.EnergyRequirements.Other,
                    OtherText = domainAggregate.EnergyRequirements.OtherText
                };

                model.FacilityRequirements = _autoMapper.Map<FacilityRequirementsViewModel>(domainAggregate.FacilityRequirements);

                return model;
            }
        }

        public class Mapper
        {
            public virtual EditHealthTechnologySpecificationInformationCommand MapToCommand(HealthTechnologySpecificationsViewModel model)
            {
                var healtTechnologySpecificationInformation = new HealthTechnologySpecificationsInformation(
                    deviceMeasurements: MapDeviceMeasurements(model),
                    useOfConsumables: MapUseOfConsumables(model),
                    estimatedLifeTime: MapEstimatedLifeTime(model),
                    estimatedShelfTime: MapEstimatedShelfTime(model),
                    canItHaveATelemedicineOrEHealthApplication: model.CanItHaveATelemedicineOrEHealthApplication,
                    deviceSoftwareUsage: MapDeviceSoftwareUsage(model),
                    portability: MapPortability(model),
                    typeOfUse: MapTypeOfUse(model),
                    maintenance: MapMaintenanceInformation(model),
                    energyRequirements: MapEnergyRequirementsInformation(model),
                    facilityRequirements: MapFacilityRequirementsInformation(model)
                );

                return new EditHealthTechnologySpecificationInformationCommand
                {
                    HealthTechnologySpecificationsInformation = healtTechnologySpecificationInformation
                };
            }

            private DeviceMeasurements MapDeviceMeasurements(HealthTechnologySpecificationsViewModel model)
            {
                return new DeviceMeasurements(
                    dimensionsHeight: model.DeviceMeasurementsViewModel.DimensionsHeight,
                    dimensionsLength: model.DeviceMeasurementsViewModel.DimensionsLength,
                    dimensionsWidth: model.DeviceMeasurementsViewModel.DimensionsWidth,
                    weightInKilograms: model.DeviceMeasurementsViewModel.WeightInKilograms
                    );
            }

            public UseOfConsumables MapUseOfConsumables(HealthTechnologySpecificationsViewModel model)
            {
                if (model.DoesItRequireUseOfConsumables == true)
                {
                    return UseOfConsumables.CreateUseOfConsumablesIfRequired(model.IfRequiresConsumablesListConsumables);
                }

                if (model.DoesItRequireUseOfConsumables == false)
                {
                    return UseOfConsumables.CreateUseOfConsumablesIfNotRequired();
                }

                return null;
            }

            public Duration MapEstimatedLifeTime(HealthTechnologySpecificationsViewModel model)
            {
                return new Duration(days: model.EstimatedLifeTimeInDays, months: model.EstimatedLifeTimeInMonths, years: model.EstimatedLifeTimeInYears);
            }

            public Duration MapEstimatedShelfTime(HealthTechnologySpecificationsViewModel model)
            {
                return new Duration(days: model.EstimatedShelfLifeInDays, months: model.EstimatedShelfLifeInMonths, years: model.EstimatedShelfLifeInYears);
            }

            public DeviceSoftwareUsage MapDeviceSoftwareUsage(HealthTechnologySpecificationsViewModel model)
            {
                if (model.DeviceSoftwareUsageViewModel.DoesItUseAnyKindOfSoftware)
                {
                    return DeviceSoftwareUsage.CreateSoftwareIsUsed(
                        description: model.DeviceSoftwareUsageViewModel.IfUsesSoftwareDescribeSoftware,
                        localUseDescription: model.DeviceSoftwareUsageViewModel.IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse
                    );
                }
                else
                {
                    return DeviceSoftwareUsage.CreateSoftwareIsNotUsed();
                }
            }

            private Portability MapPortability(HealthTechnologySpecificationsViewModel model)
            {
                if (model.PortabilityKey == null)
                {
                    return new EmptyPortability();
                }

                var portabilitySpecified = Portability.PortabilityKeyTypeMap
                    .TryGetValue(model.PortabilityKey, out Type portabilityType);

                if (portabilitySpecified)
                {
                    return (Portability)Activator.CreateInstance(type: portabilityType);
                }
                else
                {
                    return null;
                }
            }

            private TypeOfUse MapTypeOfUse(HealthTechnologySpecificationsViewModel model)
            {
                if (model.TypeOfUseKey == null)
                {
                    return new EmptyTypeOfUse();
                }

                var typeOfUseSpecified = TypeOfUse.TypeOfUseKeyTypeMap
                    .TryGetValue(model.TypeOfUseKey, out Type typeOfUseType);

                if (typeOfUseSpecified)
                {
                    return (TypeOfUse)Activator.CreateInstance(type: typeOfUseType);
                }
                else
                {
                    return null;
                }
            }

            private ProviderOfMaintenance MapProviderOfMaintenance(TechnologyMaintenanceViewModel model)
            {
                if (model.IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance == null)
                {
                    return new EmptyProviderOfMaintenance();
                }

                var isProviderOfMaintenanceSpecified = ProviderOfMaintenance.ProviderOfMaintenanceKeyTypeMap
                    .TryGetValue(model.IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance, out Type providerOfMaintenanceType);

                if (isProviderOfMaintenanceSpecified)
                {
                    if (providerOfMaintenanceType == typeof(OtherProviderOfMaintenance))
                    {
                        return new OtherProviderOfMaintenance(model.IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther);
                    }
                    else
                    {
                        return (ProviderOfMaintenance)Activator.CreateInstance(type: providerOfMaintenanceType);
                    }
                }
                else
                {
                    return null;
                }
            }

            private MaintenanceInformation MapMaintenanceInformation(HealthTechnologySpecificationsViewModel model)
            {
                if (model.TechnologyMaintenanceViewModel.DoesTechnologyRequireMaintenance)
                {
                    return MaintenanceInformation.CreateMaintenanceRequired(
                        type: model.TechnologyMaintenanceViewModel.IfTechnologyRequiresMaintenanceSpecifyType,
                        frequency: model.TechnologyMaintenanceViewModel.IfTechnologyRequiresMaintenanceSpecifyFrequency,
                        canBeDoneOnSitrOrHomeOrCommunity: model.TechnologyMaintenanceViewModel.IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity,
                        provider: MapProviderOfMaintenance(model.TechnologyMaintenanceViewModel)
                    );
                }
                else
                {
                    return MaintenanceInformation.CreateMaintenanceNotRequired();
                }
            }

            private PowerSupplyForRecharging MapPowerSupplyForRecharging(EnergyRequirementsViewModel model)
            {
                if (model.PowerSupplyForRecharging)
                {
                    return PowerSupplyForRecharging.CreatePowerSupplyForRechargingRequired(
                        voltage: model.IfPowerSupplyForRechargingThenRequiredVoltage,
                        timeToRecharge: new TimeSpan(hours: model.IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours, minutes: model.IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes, seconds: 0),
                        batteryLife: new TimeSpan(hours: model.IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours, minutes: model.IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes, seconds: 0)
                    );
                }
                else
                {
                    return PowerSupplyForRecharging.CreatePowerSupplyForRechargingNotRequired();
                }
            }

            private ContinuousPowerSupply MapContinuousPowerSupply(EnergyRequirementsViewModel model)
            {
                if (model.ContinuousPowerSupply)
                {
                    return ContinuousPowerSupply.CreateContinuousPowerSupplyRequired(voltage: model.IfContinuousPowerSupplyThenRequiredVoltage);
                }
                else
                {
                    return ContinuousPowerSupply.CreateContinuousPowerSupplyNotRequired();
                }
            }

            private SolarPower MapSolarPower(EnergyRequirementsViewModel model)
            {
                if (model.SolarPower)
                {
                    return SolarPower.CreateSolarPowerRequired(
                        timeToCharge: new TimeSpan(hours: model.IfSolarPowerThenTimeInSunlightRequiredToChargeInHours, minutes: model.IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes, seconds: 0),
                        batteryLife: new TimeSpan(hours: model.IfSolarPowerThenBatteryLifeInHours, minutes: model.IfSolarPowerThenBatteryLifeInMinutes, seconds: 0)
                    );
                }
                else
                {
                    return SolarPower.CreateSolarPowerNotRequired();
                }
            }

            private EnergyRequirementsInformation MapEnergyRequirementsInformation(HealthTechnologySpecificationsViewModel model)
            {
                return new EnergyRequirementsInformation(
                    mechanicalEnergy: model.EnergyRequirements.MechanicalEnergy,
                    batteries: model.EnergyRequirements.Batteries,
                    powerSupplyForRecharging: MapPowerSupplyForRecharging(model.EnergyRequirements),
                    continuousPowerSupply: MapContinuousPowerSupply(model.EnergyRequirements),
                    solarPower: MapSolarPower(model.EnergyRequirements),
                    other: model.EnergyRequirements.Other,
                    otherText: model.EnergyRequirements.Other ? model.EnergyRequirements.OtherText : null
                    );
            }

            private FacilityRequirementsInformation MapFacilityRequirementsInformation(HealthTechnologySpecificationsViewModel model)
            {
                return new FacilityRequirementsInformation(
                    specificTemperatureAndOrHumidityRange: model.FacilityRequirements.SpecificTemperatureAndOrHumidityRange,
                    ifSpecificTemperatureAndOrHumidityRangeThenDescription: model.FacilityRequirements.SpecificTemperatureAndOrHumidityRange ? model.FacilityRequirements.IfSpecificTemperatureAndOrHumidityRangeThenDescription : null,
                    clinicalWasteDisposalFacilities: model.FacilityRequirements.ClinicalWasteDisposalFacilities,
                    ifClinicalWasteDisposalFacilitiesThenDescription: model.FacilityRequirements.ClinicalWasteDisposalFacilities ? model.FacilityRequirements.IfClinicalWasteDisposalFacilitiesThenDescription : null,
                    gasSupply: model.FacilityRequirements.GasSupply, 
                    ifGasSupplyThenDescription: model.FacilityRequirements.GasSupply ? model.FacilityRequirements.IfGasSupplyThenDescription : null,
                    sterilization: model.FacilityRequirements.Sterilization,
                    ifSterilizationThenDescription: model.FacilityRequirements.Sterilization ? model.FacilityRequirements.IfSterilizationThenDescription : null,
                    radiationIsolation: model.FacilityRequirements.RadiationIsolation,
                    cleanWaterSupply: model.FacilityRequirements.CleanWaterSupply,
                    accessToInternet: model.FacilityRequirements.AccessToInternet,
                    accessToCellularPhoneNetwork: model.FacilityRequirements.AccessToCellularPhoneNetwork,
                    connectionToLaptopComputer: model.FacilityRequirements.ConnectionToLaptopComputer,
                    accessibleByCar: model.FacilityRequirements.AccessibleByCar,
                    additionalSoundOrLightControlFacilites: model.FacilityRequirements.AdditionalSoundOrLightControlFacilites,
                    other: model.FacilityRequirements.Other,
                    otherText: model.FacilityRequirements.Other ? model.FacilityRequirements.OtherText : null
                    );
            }

        }
    }
}
