using AutoMapper;
using Ubora.Domain.Projects.StructuredInformations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class HealthTechnologySpecificationsResult
    {
        public DeviceMeasurementsViewModel DeviceMeasurementsViewModel { get; set; }
        public bool? DoesItRequireUseOfConsumables { get; set; }
        public string IfRequiresConsumablesListConsumables { get; set; }
        public int EstimatedLifeTimeInDays { get; set; }
        public int EstimatedLifeTimeInMonths { get; set; }
        public int EstimatedLifeTimeInYears { get; set; }
        public int EstimatedShelfLifeInDays { get; set; }
        public int EstimatedShelfLifeInMonths { get; set; }
        public int EstimatedShelfLifeInYears { get; set; }
        public bool CanItHaveATelemedicineOrEHealthApplication { get; set; }
        public DeviceSoftwareUsageViewModel DeviceSoftwareUsageViewModel { get; set; }
        public string Portability { get; set; }
        public string TypeOfUse { get; set; }
        public bool DoesTechnologyRequireMaintenance { get; set; }
        public string IfTechnologyRequiresMaintenanceSpecifyType { get; set; }
        public string IfTechnologyRequiresMaintenanceSpecifyFrequency { get; set; }
        public bool IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity { get; set; }
        public string IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance { get; set; }
        public EnergyRequirementsViewModel EnergyRequirementsViewModel { get; set; }
        public FacilityRequirementsViewModel FacilityRequirementsViewModel { get; set; }

        public class Factory
        {
            private readonly IMapper _autoMapper;

            public Factory(IMapper autoMapper)
            {
                _autoMapper = autoMapper;
            }

            public HealthTechnologySpecificationsResult Create(HealthTechnologySpecificationsInformation healthTechnologySpecifications)
            {
                return new HealthTechnologySpecificationsResult
                {
                    DeviceMeasurementsViewModel = new DeviceMeasurementsViewModel
                    {
                        DimensionsWidth = healthTechnologySpecifications.DeviceMeasurements.DimensionsWidth,
                        DimensionsHeight = healthTechnologySpecifications.DeviceMeasurements.DimensionsHeight,
                        DimensionsLength = healthTechnologySpecifications.DeviceMeasurements.DimensionsLength,
                        WeightInKilograms = healthTechnologySpecifications.DeviceMeasurements.WeightInKilograms
                    },
                    DoesItRequireUseOfConsumables = healthTechnologySpecifications.UseOfConsumables?.IsRequired,
                    IfRequiresConsumablesListConsumables = healthTechnologySpecifications.UseOfConsumables?.IfRequiresConsumablesListConsumables,

                    EstimatedLifeTimeInDays = healthTechnologySpecifications.EstimatedLifeTime.Days,
                    EstimatedLifeTimeInMonths = healthTechnologySpecifications.EstimatedLifeTime.Months,
                    EstimatedLifeTimeInYears = healthTechnologySpecifications.EstimatedLifeTime.Years,

                    EstimatedShelfLifeInDays = healthTechnologySpecifications.EstimatedShelfTime.Days,
                    EstimatedShelfLifeInMonths = healthTechnologySpecifications.EstimatedShelfTime.Months,
                    EstimatedShelfLifeInYears = healthTechnologySpecifications.EstimatedShelfTime.Years,

                    CanItHaveATelemedicineOrEHealthApplication = healthTechnologySpecifications.CanItHaveATelemedicineOrEHealthApplication,

                    DeviceSoftwareUsageViewModel = new DeviceSoftwareUsageViewModel
                    {
                        DoesItUseAnyKindOfSoftware = healthTechnologySpecifications.DeviceSoftwareUsage.DoesItUseAnyKindOfSoftware,
                        IfUsesSoftwareDescribeSoftware = healthTechnologySpecifications.DeviceSoftwareUsage.IfUsesSoftwareDescribeSoftware,
                        IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = healthTechnologySpecifications.DeviceSoftwareUsage.IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse
                    },

                    Portability = healthTechnologySpecifications.Portability?.ToDisplayName(),
                    TypeOfUse = healthTechnologySpecifications.TypeOfUse?.ToDisplayName(),
                    DoesTechnologyRequireMaintenance = healthTechnologySpecifications.Maintenance.DoesTechnologyRequireMaintenance,
                    IfTechnologyRequiresMaintenanceSpecifyType = healthTechnologySpecifications.Maintenance.MaintenanceType,
                    IfTechnologyRequiresMaintenanceSpecifyFrequency = healthTechnologySpecifications.Maintenance.MaintenanceFrequency,
                    IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity = healthTechnologySpecifications.Maintenance.CanMaintenanceBeDoneOnSiteOrHomeOrCommunity,
                    IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance = healthTechnologySpecifications.Maintenance.ProviderOfMaintenance?.ToDisplayName(),
                    EnergyRequirementsViewModel = new EnergyRequirementsViewModel
                    {
                        Batteries = healthTechnologySpecifications.EnergyRequirements.Batteries,
                        ContinuousPowerSupply = healthTechnologySpecifications.EnergyRequirements.ContinuousPowerSupply.IsRequired,
                        IfContinuousPowerSupplyThenRequiredVoltage = healthTechnologySpecifications.EnergyRequirements.ContinuousPowerSupply.IfContinuousPowerSupplyThenRequiredVoltage,
                        PowerSupplyForRecharging = healthTechnologySpecifications.EnergyRequirements.PowerSupplyForRecharging.IsRequired,
                        IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours = (int) healthTechnologySpecifications.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredBatteryLife.TotalHours,
                        IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes = healthTechnologySpecifications.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredBatteryLife.Minutes,
                        IfPowerSupplyForRechargingThenRequiredVoltage = healthTechnologySpecifications.EnergyRequirements.PowerSupplyForRecharging.IfPowerSupplyForRechargingThenRequiredVoltage,
                        IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours = (int) healthTechnologySpecifications.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredTimeToRecharge.TotalHours,
                        IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes = healthTechnologySpecifications.EnergyRequirements.PowerSupplyForRecharging.PowerSupplyForRechargingRequiredTimeToRecharge.Minutes,
                        SolarPower = healthTechnologySpecifications.EnergyRequirements.SolarPower.IsRequired,
                        IfSolarPowerThenBatteryLifeInHours = (int) healthTechnologySpecifications.EnergyRequirements.SolarPower.SolarPowerBatteryLife.TotalHours,
                        IfSolarPowerThenBatteryLifeInMinutes = healthTechnologySpecifications.EnergyRequirements.SolarPower.SolarPowerBatteryLife.Minutes,
                        IfSolarPowerThenTimeInSunlightRequiredToChargeInHours = (int) healthTechnologySpecifications.EnergyRequirements.SolarPower.SolarPowerTimeInSunlightRequiredToCharge.TotalHours,
                        IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes = healthTechnologySpecifications.EnergyRequirements.SolarPower.SolarPowerTimeInSunlightRequiredToCharge.Minutes,
                        MechanicalEnergy = healthTechnologySpecifications.EnergyRequirements.MechanicalEnergy,
                        Other = healthTechnologySpecifications.EnergyRequirements.Other,
                        OtherText = healthTechnologySpecifications.EnergyRequirements.OtherText
                    },
                    FacilityRequirementsViewModel = _autoMapper.Map<FacilityRequirementsViewModel>(healthTechnologySpecifications.FacilityRequirements)
                };
            }
        }
    }
}
