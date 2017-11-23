using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public enum IntendedUser
    {
        Other = 0,
        SelfUseOrPatient,
        Physician,
        Technician,
        Nurse,
        Midwife,
        FamilyMember
    }

    public enum Portability
    {
        InstalledAndStationary,
        Mobile,
        Portable
    }

    public enum TypeOfUse
    {
        SingleUse,
        LongTermUse,
        Reusable,
        CapitalEquipment
    }

    public enum ProviderOfMaintenance
    {
        Other = 0,
        SelfUserOrPatient,
        NurseOrPhysician,
        Engineer,
        Manufacturer,
        Technician
    }

    public class DeviceStructuredInformationViewModel
    {
        public IntendedUser IntendedUser { get; set; }
        public string IntendedUserOther { get; set; }
        public bool IsTrainingRequiredInAdditionToTheExpectedSkillLevelOfTheIntentedUser { get; set; }
        public string IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTheTrainingAndTheMaterialsAndTheTimeRequiredForTheTraining { get; set; }
        public bool IsAnyMaintenanceOrCalibrationRequiredByTheUserAtTheTimeOfUse { get; set; }
        public WhereWillTheTechnologyBeUsedViewModel WhereWillTheTechnologyBeUsed { get; set; }
        public decimal DimensionsHeight { get; set; }
        public decimal DimensionsLength { get; set; }
        public decimal DimensionsWidth { get; set; }
        public decimal Weight { get; set; }
        public bool DoesItRequireTheUseOfConsumables { get; set; }
        public string IfRequiresConsumablesListTheConsumables { get; set; }
        public int EstimatedLifeTimeInDays { get; set; }
        public int EstimatedLifeTimeInMonths { get; set; }
        public int EstimatedLifeTimeInYears { get; set; }
        public int EstimatedShelfLifeInDays { get; set; }
        public int EstimatedShelfLifeInMonths { get; set; }
        public int EstimatedShelfLifeInYears { get; set; }
        public bool CanItHaveATelemedicineOrEHealthApplication { get; set; }
        public bool DoesItUseAnyKindOfSoftware { get; set; }
        public string IfUsesSoftwareDescribeTheSoftware { get; set; }
        public string IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse { get; set; }
        public Portability IsItPortable { get; set; }
        public TypeOfUse TypeOfUse { get; set; }
        public bool DoesTheTechnologyRequireMaintenance { get; set; }
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

    public class WhereWillTheTechnologyBeUsedViewModel
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
