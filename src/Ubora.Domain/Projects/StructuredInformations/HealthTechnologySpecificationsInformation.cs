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
            UseOfConsumables useOfConsumables,
            Duration estimatedLifeTime,
            Duration estimatedShelfTime, 
            bool canItHaveATelemedicineOrEHealthApplication, 
            DeviceSoftwareUsage deviceSoftwareUsage, 
            Portability portability, 
            TypeOfUse typeOfUse, 
            MaintenanceInformation maintenance, 
            EnergyRequirementsInformation energyRequirements, 
            FacilityRequirementsInformation facilityRequirements)
        {
            DeviceMeasurements = deviceMeasurements ?? throw new ArgumentNullException(nameof(deviceMeasurements));
            UseOfConsumables = useOfConsumables;
            EstimatedLifeTime = estimatedLifeTime ?? throw new ArgumentNullException(nameof(estimatedLifeTime));
            EstimatedShelfTime = estimatedShelfTime ?? throw new ArgumentNullException(nameof(estimatedShelfTime));
            CanItHaveATelemedicineOrEHealthApplication = canItHaveATelemedicineOrEHealthApplication;
            DeviceSoftwareUsage = deviceSoftwareUsage ?? throw new ArgumentNullException(nameof(deviceSoftwareUsage));
            Portability = portability ?? throw new ArgumentNullException(nameof(portability));
            TypeOfUse = typeOfUse ?? throw new ArgumentNullException(nameof(typeOfUse));
            Maintenance = maintenance ?? throw new ArgumentNullException(nameof(maintenance));
            EnergyRequirements = energyRequirements ?? throw new ArgumentNullException(nameof(energyRequirements));
            FacilityRequirements = facilityRequirements ?? throw new ArgumentNullException(nameof(facilityRequirements));    
        }

        [JsonConstructor]
        public HealthTechnologySpecificationsInformation()
        {
        }

        public DeviceMeasurements DeviceMeasurements { get; private set; } = DeviceMeasurements.CreateEmpty();
        public UseOfConsumables UseOfConsumables { get; private set; } = UseOfConsumables.CreateEmpty();
        public Duration EstimatedLifeTime { get; private set; } = Duration.CreateEmpty();
        public Duration EstimatedShelfTime { get; private set; } = Duration.CreateEmpty();
        public bool CanItHaveATelemedicineOrEHealthApplication { get; private set; }
        public DeviceSoftwareUsage DeviceSoftwareUsage { get; private set; } = DeviceSoftwareUsage.CreateEmpty();
        public Portability Portability { get; private set; } = new EmptyPortability();
        public TypeOfUse TypeOfUse { get; private set; } = new EmptyTypeOfUse();
        public MaintenanceInformation Maintenance { get; private set; } = MaintenanceInformation.CreateEmpty();

        public EnergyRequirementsInformation EnergyRequirements { get; private set; } = EnergyRequirementsInformation.CreateEmpty();
        public FacilityRequirementsInformation FacilityRequirements { get; private set; } = FacilityRequirementsInformation.CreateEmpty();

        public static HealthTechnologySpecificationsInformation CreateEmpty()
        {
            return new HealthTechnologySpecificationsInformation();
        }
    }
}