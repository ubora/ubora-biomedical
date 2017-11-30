using System;
using Newtonsoft.Json;
using Ubora.Domain.Projects.StructuredInformations.Portabilities;
using Ubora.Domain.Projects.StructuredInformations.TypesOfUse;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class HealthTechnologySpecificationsInformation
    {
        public HealthTechnologySpecificationsInformation(
            DeviceMeasurements deviceMeasurements, 
            bool doesItRequireUseOfConsumables, 
            string ifRequiresConsumablesListConsumables, 
            TimeSpan estimatedLifeTime, 
            TimeSpan estimatedShelfTime, 
            bool canItHaveATelemedicineOrEHealthApplication, 
            DeviceSoftwareUsage deviceSoftwareUsage, 
            Portability portability, 
            TypeOfUse typeOfUse, 
            MaintenanceInformation maintenance, 
            EnergyRequirementsInformation energyRequirements, 
            FacilityRequirementsInformation facilityRequirements)
        {
            DeviceMeasurements = deviceMeasurements ?? throw new ArgumentNullException(nameof(deviceMeasurements));
            DoesItRequireUseOfConsumables = doesItRequireUseOfConsumables;
            IfRequiresConsumablesListConsumables = ifRequiresConsumablesListConsumables;
            EstimatedLifeTime = estimatedLifeTime;
            EstimatedShelfTime = estimatedShelfTime;
            CanItHaveATelemedicineOrEHealthApplication = canItHaveATelemedicineOrEHealthApplication;
            DeviceSoftwareUsage = deviceSoftwareUsage ?? throw new ArgumentNullException(nameof(deviceSoftwareUsage));
            Portability = portability ?? throw new ArgumentNullException(nameof(portability));
            TypeOfUse = typeOfUse ?? throw new ArgumentNullException(nameof(typeOfUse));
            Maintenance = maintenance ?? throw new ArgumentNullException(nameof(maintenance));
            EnergyRequirements = energyRequirements ?? throw new ArgumentNullException(nameof(energyRequirements));
            FacilityRequirements = facilityRequirements ?? throw new ArgumentNullException(nameof(facilityRequirements));    
        }

        [JsonConstructor]
        protected HealthTechnologySpecificationsInformation()
        {
        }

        public DeviceMeasurements DeviceMeasurements { get; private set; } = DeviceMeasurements.CreateEmpty();
        public bool DoesItRequireUseOfConsumables { get; private set; }
        public string IfRequiresConsumablesListConsumables { get; private set; }
        public TimeSpan EstimatedLifeTime { get; private set; }
        public TimeSpan EstimatedShelfTime { get; private set; }
        public bool CanItHaveATelemedicineOrEHealthApplication { get; private set; }
        public DeviceSoftwareUsage DeviceSoftwareUsage { get; private set; } = DeviceSoftwareUsage.CreateEmpty();
        public Portability Portability { get; private set; } = new EmptyPortability();
        public TypeOfUse TypeOfUse { get; private set; } = new EmptyTypeOfUse();
        public MaintenanceInformation Maintenance { get; private set; } = MaintenanceInformation.CreateEmpty();

        public EnergyRequirementsInformation EnergyRequirements { get; private set; }
        public FacilityRequirementsInformation FacilityRequirements { get; private set; } = FacilityRequirementsInformation.CreateEmpty();

        public static HealthTechnologySpecificationsInformation CreateEmpty()
        {
            return new HealthTechnologySpecificationsInformation();
        }
    }
}