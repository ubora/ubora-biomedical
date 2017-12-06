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
        public int IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        public int IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        public int IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours { get; set; }
        [RequiredIf(nameof(PowerSupplyForRecharging), true)]
        public int IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes { get; set; }
        public bool ContinuousPowerSupply { get; set; }
        [RequiredIf(nameof(ContinuousPowerSupply), true)]
        public decimal IfContinuousPowerSupplyThenRequiredVoltage { get; set; }
        public bool SolarPower { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        public int IfSolarPowerThenTimeInSunlightRequiredToChargeInHours { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        public int IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        public int IfSolarPowerThenBatteryLifeInHours { get; set; }
        [RequiredIf(nameof(SolarPower), true)]
        public int IfSolarPowerThenBatteryLifeInMinutes { get; set; }
        public bool Other { get; set; }
        [RequiredIf(nameof(Other), true)]
        public string OtherText { get; set; }
    }
}