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
        public DeviceMeasurements()
        {
        }

        public decimal DimensionsHeight { get; private set; }
        public decimal DimensionsLength { get; private set; }
        public decimal DimensionsWidth { get; private set; }
        public decimal WeightInKilograms { get; private set; }

        public static DeviceMeasurements CreateEmpty()
        {
            return new DeviceMeasurements();
        }
    }
}