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

        public bool SpecificTemperatureAndOrHumidityRange { get; set; }
        public string IfSpecificTemperatureAndOrHumidityRangeThenDescription { get; set; }

        public bool ClinicalWasteDisposalFacilities { get; set; }
        public string IfClinicalWasteDisposalFacilitiesThenDescription { get; set; }

        public bool GasSupply { get; set; }
        public string IfGasSupplyThenDescription { get; set; }

        public bool Sterilization { get; set; }
        public string IfSterilizationThenDescription { get; set; }

        public bool RadiationIsolation { get; set; }
        public bool CleanWaterSupply { get; set; }
        public bool AccessToInternet { get; set; }
        public bool AccessToCellularPhoneNetwork { get; set; }
        public bool ConnectionToLaptopComputer { get; set; }
        public bool AccessibleByCar { get; set; }
        public bool AdditionalSoundOrLightControlFacilites { get; set; }

        public bool Other { get; set; }
        public string OtherText { get; set; }

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