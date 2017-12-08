using System.Collections.Generic;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class FacilityRequirementsViewModel
    {
        public bool CleanWaterSupply { get; set; }
        public bool SpecificTemperatureAndOrHumidityRange { get; set; }
        [RequiredIf(nameof(SpecificTemperatureAndOrHumidityRange), true)]
        public string IfSpecificTemperatureAndOrHumidityRangeThenDescription { get; set; }
        public bool ClinicalWasteDisposalFacilities { get; set; }
        [RequiredIf(nameof(ClinicalWasteDisposalFacilities), true)]
        public string IfClinicalWasteDisposalFacilitiesThenDescription { get; set; }
        public bool RadiationIsolation { get; set; }
        public bool GasSupply { get; set; }
        [RequiredIf(nameof(GasSupply), true)]
        public string IfGasSupplyThenDescription { get; set; }
        public bool Sterilization { get; set; }
        [RequiredIf(nameof(Sterilization), true)]
        public string IfSterilizationThenDescription { get; set; }
        public bool AccessToInternet { get; set; }
        public bool AccessToCellularPhoneNetwork { get; set; }
        public bool ConnectionToLaptopComputer { get; set; }
        public bool AccessibleByCar { get; set; }
        public bool AdditionalSoundOrLightControlFacilites { get; set; }
        public bool Other { get; set; }
        [RequiredIf(nameof(Other), true)]
        public string OtherText { get; set; }

        public IEnumerable<string> GetResult()
        {
            if (CleanWaterSupply)
            {
                yield return "Clean water supply";
            }

            if (SpecificTemperatureAndOrHumidityRange)
            {
                var result = "Specific temperature/humidity range";
                if (!string.IsNullOrEmpty(IfSpecificTemperatureAndOrHumidityRangeThenDescription))
                {
                    result += $" (description: {IfSpecificTemperatureAndOrHumidityRangeThenDescription})";
                }
                yield return result;
            }

            if (ClinicalWasteDisposalFacilities)
            {
                var result = "Clinical waste disposal facilities";
                if (!string.IsNullOrEmpty(IfClinicalWasteDisposalFacilitiesThenDescription))
                {
                    result += $" (description: {IfClinicalWasteDisposalFacilitiesThenDescription})";
                }
                yield return result;
            }

            if (GasSupply)
            {
                var result = "Gas supply";
                if (!string.IsNullOrEmpty(IfGasSupplyThenDescription))
                {
                    result += $" (description: {IfGasSupplyThenDescription})";
                }
                yield return result;
            }

            if (Sterilization)
            {
                var result = "Sterilization";
                if (!string.IsNullOrEmpty(IfSterilizationThenDescription))
                {
                    result += $" (description: {IfSterilizationThenDescription})";
                }
                yield return result;
            }

            if (RadiationIsolation)
            {
                yield return "Radiation isolation";
            }

            if (AccessToInternet)
            {
                yield return "Access to the Internet";
            }

            if (AccessToCellularPhoneNetwork)
            {
                yield return "Access to a cellular phone network";
            }

            if (ConnectionToLaptopComputer)
            {
                yield return "Connection to a laptop/computer";
            }

            if (AccessibleByCar)
            {
                yield return "Accessible by car";
            }

            if (AdditionalSoundOrLightControlFacilites)
            {
                yield return "Additional sound / light control facilities";
            }

            if (Other)
            {
                yield return OtherText;
            }
        }
    }
}