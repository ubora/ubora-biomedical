using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class UseOfConsumables
    {
        [JsonConstructor]
        public UseOfConsumables()
        {
        }

        public static UseOfConsumables CreateUseOfConsumablesIfRequired(string consumables)
        {
            return new UseOfConsumables
            {
                IsRequired = true,
                IfRequiresConsumablesListConsumables = consumables
            };
        }

        public static UseOfConsumables CreateUseOfConsumablesIfNotRequired()
        {
            return new UseOfConsumables
            {
                IsRequired = false
            };
        }

        public bool IsRequired { get; private set; }
        public string IfRequiresConsumablesListConsumables { get; private set; }
    }
}