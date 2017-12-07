using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class EnergyRequirementsViewModel
    {
        public bool MechanicalEnergy { get; set; }
        public bool Batteries { get; set; }
        public bool PowerSupplyForRecharging { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        public decimal IfPowerSupplyForRechargingThenRequiredVoltage { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        [Range(0,10000, ErrorMessage = "Value for hours must be between {1} and {2}")]
        public int IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        [Range(0, 59, ErrorMessage = "Value for minutes must be between {1} and {2}")]
        public int IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        [Range(0, 10000, ErrorMessage = "Value for hours must be between {1} and {2}")]
        public int IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        [Range(0, 59, ErrorMessage = "Value for minutes must be between {1} and {2}")]
        public int IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes { get; set; }
        public bool ContinuousPowerSupply { get; set; }
        [RequiredIf(nameof(ContinuousPowerSupply), true)]
        public decimal IfContinuousPowerSupplyThenRequiredVoltage { get; set; }
        public bool SolarPower { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        [Range(0, 10000, ErrorMessage = "Value for hours must be between {1} and {2}")]
        public int IfSolarPowerThenTimeInSunlightRequiredToChargeInHours { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        [Range(0, 59, ErrorMessage = "Value for minutes must be between {1} and {2}")]
        public int IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        [Range(0, 10000, ErrorMessage = "Value for hours must be between {1} and {2}")]
        public int IfSolarPowerThenBatteryLifeInHours { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        [Range(0, 59, ErrorMessage = "Value for minutes must be between {1} and {2}")]
        public int IfSolarPowerThenBatteryLifeInMinutes { get; set; }
        public bool Other { get; set; }
        [RequiredIf(nameof(Other), true)]
        public string OtherText { get; set; }
    }
}