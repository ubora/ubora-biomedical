﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Projects.StructuredInformations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{

    public class DeviceStructuredInformationViewModel
    {
        public IntendedUser IntendedUser { get; set; }
        public string IntendedUserOther { get; set; }
        public bool IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser { get; set; }
        public string IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining { get; set; }
        public bool IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse { get; set; }
        public WhereWillTechnologyBeUsedViewModel WhereWillTechnologyBeUsed { get; set; }
        public decimal DimensionsHeight { get; set; }
        public decimal DimensionsLength { get; set; }
        public decimal DimensionsWidth { get; set; }
        public decimal WeightInKilograms { get; set; }
        public bool DoesItRequireUseOfConsumables { get; set; }
        public string IfRequiresConsumablesListConsumables { get; set; }
        public int EstimatedLifeTimeInDays { get; set; }
        public int EstimatedLifeTimeInMonths { get; set; }
        public int EstimatedLifeTimeInYears { get; set; }
        public int EstimatedShelfLifeInDays { get; set; }
        public int EstimatedShelfLifeInMonths { get; set; }
        public int EstimatedShelfLifeInYears { get; set; }
        public bool CanItHaveATelemedicineOrEHealthApplication { get; set; }
        public bool DoesItUseAnyKindOfSoftware { get; set; }
        public string IfUsesSoftwareDescribeSoftware { get; set; }
        public string IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse { get; set; }
        public Portability IsItPortable { get; set; }
        public TypeOfUse TypeOfUse { get; set; }
        public bool DoesTechnologyRequireMaintenance { get; set; }
        public string IfTechnologyRequiresMaintenanceSpecifyType { get; set; }
        public string IfTechnologyRequiresMaintenanceSpecifyFrequency { get; set; }
        public bool IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity { get; set; }
        public ProviderOfMaintenance IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenance { get; set; }
        public string IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther { get; set; }
        public EnergyRequirementsViewModel EnergyRequirements { get; set; }
        public FacilityRequirementsViewModel FacilityRequirements { get; set; }
    }

    public class EnergyRequirementsViewModel
    {
        public bool MechanicalEnergy { get; set; }
        public bool Batteries { get; set; }
        public bool PowerSupplyForRecharging { get; set; }
        public decimal IfPowerSupplyForRechargingThenRequiredVoltage { get; set; }
        public int IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours { get; set; }
        public int IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes { get; set; }
        public int IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours { get; set; }
        public int IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes { get; set; }
        public bool ContinuousPowerSupply { get; set; }
        public decimal IfContinuousPowerSupplyThenRequiredVoltage { get; set; }
        public bool SolarPower { get; set; }
        public int IfSolarPowerThenTimeInSunlightRequiredToChargeInHours { get; set; }
        public int IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes { get; set; }
        public int IfSolarPowerThenBatteryLifeInHours { get; set; }
        public int IfSolarPowerThenBatteryLifeInMinutes { get; set; }
        public bool Other { get; set; }
        public string OtherText { get; set; }
    }

    public class FacilityRequirementsViewModel
    {
        public bool CleanWaterSupply { get; set; }
        public bool SpecificTemperatureAndOrHumidityRange { get; set; }
        public string IfSpecificTemperatureAndOrHumidityRangeThenDescription { get; set; }
        public bool ClinicalWasteDisposalFacilities { get; set; }
        public string IfClinicalWasteDisposalFacilitiesThenDescription { get; set; }
        public bool RadiationIsolation { get; set; }
        public bool GasSupply { get; set; }
        public string IfGasSupplyThenDescription { get; set; }
        public bool Sterilization { get; set; }
        public string IfSterilizationThenDescription { get; set; }
        public bool AccessToInternet { get; set; }
        public bool AccessToCellularPhoneNetwork { get; set; }
        public bool ConnectionToLaptopComputer { get; set; }
        public bool AccessibleByCar { get; set; }
        public bool AdditionalSoundOrLightControlFacilites { get; set; }
        public bool Other { get; set; }
        public string OtherText { get; set; }
    }

    public class WhereWillTechnologyBeUsedViewModel
    {
        public bool RuralSettings { get; set; }
        public bool UrbanSettings { get; set; }
        public bool Outdoors { get; set; }
        public bool Indoors { get; set; }
        public bool AtHome { get; set; }
        public bool PrimaryLevel { get; set; }
        public bool SecondaryLevel { get; set; }
        public bool TertiaryLevel { get; set; }
    }
}
