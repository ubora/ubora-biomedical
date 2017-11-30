using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class DeviceMeasurements
    {
        public DeviceMeasurements(decimal dimensionsHeight, decimal dimensionsLength, decimal dimensionsWidth, decimal weightInKilograms)
        {
            DimensionsHeight = dimensionsHeight;
            DimensionsLength = dimensionsLength;
            DimensionsWidth = dimensionsWidth;
            WeightInKilograms = weightInKilograms;
        }

        [JsonConstructor]
        private DeviceMeasurements()
        {
        }

        public decimal DimensionsHeight { get; set; }
        public decimal DimensionsLength { get; set; }
        public decimal DimensionsWidth { get; set; }
        public decimal WeightInKilograms { get; set; }

        public static DeviceMeasurements CreateEmpty()
        {
            return new DeviceMeasurements();
        }
    }
}