using System;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class EnergyRequirementsInformation
    {
        public bool MechanicalEnergy { get; set; }
        public bool Batteries { get; set; }

        public PowerSupplyForRecharging PowerSupplyForRecharging { get; set; }

        public bool ContinuousPowerSupply { get; set; }
        public decimal IfContinuousPowerSupplyThenRequiredVoltage { get; set; }

        public bool SolarPower { get; set; }
        public TimeSpan? SolarPowerTimeInSunlightRequiredToCharge { get; set; }
        public TimeSpan? SolarPowerBatteryLife { get; set; }

        public bool Other { get; set; }
        public string OtherText { get; set; }
    }

    public class PowerSupplyForRecharging
    {
        public bool IsRequired { get; set; }
        public decimal IfPowerSupplyForRechargingThenRequiredVoltage { get; set; }
        public TimeSpan PowerSupplyForRechargingRequiredTimeToRecharge { get; set; }
        public TimeSpan PowerSupplyForRechargingRequiredBatteryLife { get; set; }
    }

}