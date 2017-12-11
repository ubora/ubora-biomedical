using System;
using Newtonsoft.Json;
using Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class MaintenanceInformation
    {
        public MaintenanceInformation(bool doesTechnologyRequireMaintenance, string maintenanceType, string maintenanceFrequency, bool canMaintenanceBeDoneOnSiteOrHomeOrCommunity, ProviderOfMaintenance providerOfMaintenance)
        {
            DoesTechnologyRequireMaintenance = doesTechnologyRequireMaintenance;
            MaintenanceType = maintenanceType;
            MaintenanceFrequency = maintenanceFrequency;
            CanMaintenanceBeDoneOnSiteOrHomeOrCommunity = canMaintenanceBeDoneOnSiteOrHomeOrCommunity;
            ProviderOfMaintenance = providerOfMaintenance ?? throw new ArgumentNullException(nameof(providerOfMaintenance));
        }
        
        [JsonConstructor]
        public MaintenanceInformation()
        {
        }

        public bool DoesTechnologyRequireMaintenance { get; private set; }
        public string MaintenanceType { get; private set; }
        public string MaintenanceFrequency { get; private set; }
        public bool CanMaintenanceBeDoneOnSiteOrHomeOrCommunity { get; private set; }
        public ProviderOfMaintenance ProviderOfMaintenance { get; private set; } = new EmptyProviderOfMaintenance();

        public static MaintenanceInformation CreateMaintenanceRequired(string type, string frequency, bool canBeDoneOnSitrOrHomeOrCommunity, ProviderOfMaintenance provider)
        {
            return new MaintenanceInformation
            {
                DoesTechnologyRequireMaintenance = true,
                MaintenanceType = type,
                MaintenanceFrequency = frequency,
                CanMaintenanceBeDoneOnSiteOrHomeOrCommunity = true,
                ProviderOfMaintenance = provider
            };
        }

        public static MaintenanceInformation CreateMaintenanceNotRequired()
        {
            return new MaintenanceInformation
            {
                DoesTechnologyRequireMaintenance = false
            };
        }

        public static MaintenanceInformation CreateEmpty()
        {
            return new MaintenanceInformation();
        }
    }
}