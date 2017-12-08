using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class ContinuousPowerSupply
    {
        [JsonConstructor]
        private ContinuousPowerSupply()
        {
        }

        public bool IsRequired { get; private set; }
        public decimal IfContinuousPowerSupplyThenRequiredVoltage { get; private set; }

        public static ContinuousPowerSupply CreateContinuousPowerSupplyRequired(decimal voltage)
        {
            return new ContinuousPowerSupply
            {
                IsRequired = true,
                IfContinuousPowerSupplyThenRequiredVoltage = voltage
            };
        }

        public static ContinuousPowerSupply CreateContinuousPowerSupplyNotRequired()
        {
            return new ContinuousPowerSupply
            {
                IsRequired = false
            };
        }

        public static ContinuousPowerSupply CreateEmpty()
        {
            return new ContinuousPowerSupply();
        }
    }
}