using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class EnergyRequirementsInformation
    {
        public EnergyRequirementsInformation(
            bool mechanicalEnergy, 
            bool batteries, 
            PowerSupplyForRecharging powerSupplyForRecharging, 
            ContinuousPowerSupply continuousPowerSupply, 
            SolarPower solarPower, 
            bool other, 
            string otherText)
        {
            MechanicalEnergy = mechanicalEnergy;
            Batteries = batteries;
            PowerSupplyForRecharging = powerSupplyForRecharging ?? throw new ArgumentNullException(nameof(powerSupplyForRecharging));
            ContinuousPowerSupply = continuousPowerSupply ?? throw new ArgumentNullException(nameof(continuousPowerSupply));
            SolarPower = solarPower ?? throw new ArgumentNullException(nameof(solarPower));
            Other = other;
            OtherText = otherText;
        }

        [JsonConstructor]
        public EnergyRequirementsInformation()
        {
        }

        public bool MechanicalEnergy { get; private set; }
        public bool Batteries { get; private set; }

        public PowerSupplyForRecharging PowerSupplyForRecharging { get; private set; } = PowerSupplyForRecharging.CreateEmpty();
        public ContinuousPowerSupply ContinuousPowerSupply { get; private set; } = ContinuousPowerSupply.CreateEmpty();
        public SolarPower SolarPower { get; private set; } = SolarPower.CreateEmpty();

        public bool Other { get; private set; }
        public string OtherText { get; private set; }

        public static EnergyRequirementsInformation CreateEmpty()
        {
            return new EnergyRequirementsInformation();
        }
    }
}