using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class DeviceMeasurementsViewModel
    {
        [Required]
        public decimal DimensionsHeight { get; set; }
        [Required]
        public decimal DimensionsLength { get; set; }
        [Required]
        public decimal DimensionsWidth { get; set; }
        [Required]
        public decimal WeightInKilograms { get; set; }

        public bool IsAllDimensionsSet => DimensionsLength != 0 || DimensionsHeight != 0 || DimensionsWidth != 0;
    }
}