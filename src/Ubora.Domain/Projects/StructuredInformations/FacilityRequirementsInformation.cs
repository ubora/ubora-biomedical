using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class FacilityRequirementsInformation
    {
        public FacilityRequirementsInformation(
            bool specificTemperatureAndOrHumidityRange, 
            string ifSpecificTemperatureAndOrHumidityRangeThenDescription, 
            bool clinicalWasteDisposalFacilities, 
            string ifClinicalWasteDisposalFacilitiesThenDescription, 
            bool gasSupply, 
            string ifGasSupplyThenDescription, 
            bool sterilization, 
            string ifSterilizationThenDescription, 
            bool radiationIsolation, 
            bool cleanWaterSupply, 
            bool accessToInternet, 
            bool accessToCellularPhoneNetwork, 
            bool connectionToLaptopComputer, 
            bool accessibleByCar, 
            bool additionalSoundOrLightControlFacilites, 
            bool other, 
            string otherText)
        {
            SpecificTemperatureAndOrHumidityRange = specificTemperatureAndOrHumidityRange;
            IfSpecificTemperatureAndOrHumidityRangeThenDescription = ifSpecificTemperatureAndOrHumidityRangeThenDescription;
            ClinicalWasteDisposalFacilities = clinicalWasteDisposalFacilities;
            IfClinicalWasteDisposalFacilitiesThenDescription = ifClinicalWasteDisposalFacilitiesThenDescription;
            GasSupply = gasSupply;
            IfGasSupplyThenDescription = ifGasSupplyThenDescription;
            Sterilization = sterilization;
            IfSterilizationThenDescription = ifSterilizationThenDescription;
            RadiationIsolation = radiationIsolation;
            CleanWaterSupply = cleanWaterSupply;
            AccessToInternet = accessToInternet;
            AccessToCellularPhoneNetwork = accessToCellularPhoneNetwork;
            ConnectionToLaptopComputer = connectionToLaptopComputer;
            AccessibleByCar = accessibleByCar;
            AdditionalSoundOrLightControlFacilites = additionalSoundOrLightControlFacilites;
            Other = other;
            OtherText = otherText;
        }

        [JsonConstructor]
        public FacilityRequirementsInformation()
        {
        }

        public bool SpecificTemperatureAndOrHumidityRange { get; private set; }
        public string IfSpecificTemperatureAndOrHumidityRangeThenDescription { get; private set; }

        public bool ClinicalWasteDisposalFacilities { get; private set; }
        public string IfClinicalWasteDisposalFacilitiesThenDescription { get; private set; }

        public bool GasSupply { get; private set; }
        public string IfGasSupplyThenDescription { get; private set; }

        public bool Sterilization { get; private set; }
        public string IfSterilizationThenDescription { get; private set; }

        public bool RadiationIsolation { get; private set; }
        public bool CleanWaterSupply { get; private set; }
        public bool AccessToInternet { get; private set; }
        public bool AccessToCellularPhoneNetwork { get; private set; }
        public bool ConnectionToLaptopComputer { get; private set; }
        public bool AccessibleByCar { get; private set; }
        public bool AdditionalSoundOrLightControlFacilites { get; private set; }

        public bool Other { get; private set; }
        public string OtherText { get; private set; }

        public static FacilityRequirementsInformation CreateEmpty()
        {
            return new FacilityRequirementsInformation();
        }

        //public IEnumerable<FacilityRequirement> FacilityRequirements { get; set; 
    }

    //public abstract class FacilityRequirement
    //{
    //    public abstract string Name { get; }
    //    public abstract string Key { get;  }
    //}

    //public class FacilityRequirementWithoutDescription : FacilityRequirement
    //{
    //    public override string Name { get; }
    //    public override string Key { get; }
    //}

    //public class FacilityRequirementWithDescription : FacilityRequirement
    //{
    //    public override string Name { get; }
    //    public override string Key { get; }
    //    public string Description { get; set; }
    //}
}