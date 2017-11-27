using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;

namespace Ubora.Domain.Projects.StructuredInformations
{

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

        SelfUseOrPatient,
        NurseOrPhysician,
        Engineer,
        Manufacturer,
        Technician
    }

    public class DeviceStructuredInformation
    {
        public UserAndEnvironmentInformation UserAndEnvironment { get; set; }

        public class UserAndEnvironmentInformation
        {
            public IntendedUser IntendedUser { get; set; }

            public bool IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser { get; set; }
            public string DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTrainingIntendedUser { get; set; }
            public bool IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse { get; set; }

            public WhereWillTechnologyBeUsedInformation WhereWillTechnologyBeUsed { get; set; }

            public class WhereWillTechnologyBeUsedInformation
            {
                public bool RuralSettings { get; set; }
                public bool UrbanSettings { get; set; }
                public bool Outdoors { get; set; }
                public bool Indoors { get; set; }
                public bool AtHome { get; set; }
                // (health post, health center)
                public bool PrimaryLevel { get; set; }
                // (general hospital)
                public bool SecondaryLevel { get; set; }
                // (specialist hospital)
                public bool TertiaryLevel { get; set; }
            }
        }

        public HealthTechnologySpecificationsInformation HealthTechnologySpecification { get; set; }

        public class HealthTechnologySpecificationsInformation
        {
            public decimal DimensionsHeight { get; set; }
            public decimal DimensionsLength { get; set; }
            public decimal DimensionsWidth { get; set; }

            public decimal WeightInKilograms { get; set; }

            public bool DoesItRequireUseOfConsumables { get; set; }
            public string IfRequiresConsumablesListConsumables { get; set; }

            public TimeSpan EstimatedLifeTime { get; set; }
            public TimeSpan EstimatedShelfTime { get; set; }

            public bool CanItHaveATelemedicineOrEHealthApplication { get; set; }
            public bool DoesItUseAnyKindOfSoftware { get; set; }
            public string IfUsesSoftwareDescribeSoftware { get; set; }
            public string IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse { get; set; }

            public Portability Portability { get; set; }
            public TypeOfUse TypeOfUse { get; set; }

            public MaintenanceInformation Maintenance { get; set; }

            public class MaintenanceInformation
            {
                public bool DoesTechnologyRequireMaintenance { get; set; }
                public string MaintenanceType { get; set; }
                public string MaintenanceFrequency { get; set; }
                public bool CanMaintenanceBeDoneOnSiteOrHomeOrCommunity { get; set; }
                public string ProviderOfMaintenance { get; set; }
            }

            public EnergyRequirementsInformation EnergyRequirements { get; set; }

            public class EnergyRequirementsInformation
            {
                public bool MechanicalEnergy { get; set; }
                public bool Batteries { get; set; }

                public bool PowerSupplyForRecharging { get; set; }
                public decimal IfPowerSupplyForRechargingThenRequiredVoltage { get; set; }
                public TimeSpan PowerSupplyForRechargingRequiredTimeToRecharge { get; set; }
                public TimeSpan PowerSupplyForRechargingRequiredBatteryLife { get; set; }

                public bool ContinuousPowerSupply { get; set; }
                public decimal IfContinuousPowerSupplyThenRequiredVoltage { get; set; }

                public bool SolarPower { get; set; }
                public TimeSpan? SolarPowerTimeInSunlightRequiredToCharge { get; set; }
                public TimeSpan? SolarPowerBatteryLife { get; set; }

                public bool Other { get; set; }
                public string OtherText { get; set; }
            }

            public FacilityRequirementsInformation FacilityRequirements { get; set; }

            public class FacilityRequirementsInformation
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
        }
    }
}
