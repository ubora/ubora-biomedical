using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class PowerSupplyForRecharging
    {
        [JsonConstructor]
        private PowerSupplyForRecharging()
        {
        }

        public bool IsRequired { get; set; }
        public decimal IfPowerSupplyForRechargingThenRequiredVoltage { get; private set; }
        public TimeSpan PowerSupplyForRechargingRequiredTimeToRecharge { get; private set; }
        public TimeSpan PowerSupplyForRechargingRequiredBatteryLife { get; private set; }

        public static PowerSupplyForRecharging CreatePowerSupplyForRechargingRequired(decimal voltage, TimeSpan timeToRecharge, TimeSpan batteryLife)
        {
            return new PowerSupplyForRecharging
            {
                IsRequired = true,
                IfPowerSupplyForRechargingThenRequiredVoltage = voltage,
                PowerSupplyForRechargingRequiredTimeToRecharge = timeToRecharge,
                PowerSupplyForRechargingRequiredBatteryLife = batteryLife
            };
        }

        public static PowerSupplyForRecharging CreatePowerSupplyForRechargingNotRequired()
        {
            return new PowerSupplyForRecharging
            {
                IsRequired = false
            };
        }

        public static PowerSupplyForRecharging CreateEmpty()
        {
            return new PowerSupplyForRecharging();
        }
    }
}