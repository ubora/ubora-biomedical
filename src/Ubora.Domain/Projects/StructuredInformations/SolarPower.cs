using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class SolarPower
    {
        [JsonConstructor]
        private SolarPower()
        {
        }

        public bool IsRequired { get; private set; }
        public TimeSpan SolarPowerTimeInSunlightRequiredToCharge { get; private set; }
        public TimeSpan SolarPowerBatteryLife { get; private set; }

        public static SolarPower CreateSolarPowerRequired(TimeSpan timeToCharge, TimeSpan batteryLife)
        {
            return new SolarPower
            {
                IsRequired = true,
                SolarPowerTimeInSunlightRequiredToCharge = timeToCharge,
                SolarPowerBatteryLife = batteryLife
            };
        }

        public static SolarPower CreateSolarPowerNotRequired()
        {
            return new SolarPower
            {
                IsRequired = false
            };
        }

        public static SolarPower CreateEmpty()
        {
            return new SolarPower();
        }
    }
}