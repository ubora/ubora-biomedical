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

        public bool DoesTechnologyRequireMaintenance { get; set; }
        public string MaintenanceType { get; set; }
        public string MaintenanceFrequency { get; set; }
        public bool CanMaintenanceBeDoneOnSiteOrHomeOrCommunity { get; set; }
        public ProviderOfMaintenance ProviderOfMaintenance { get; set; } = new EmptyProviderOfMaintenance();

        public static MaintenanceInformation CreateEmpty()
        {
            return new MaintenanceInformation();
        }
    }
}