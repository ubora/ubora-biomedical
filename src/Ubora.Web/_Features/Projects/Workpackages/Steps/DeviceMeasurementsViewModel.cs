using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class DeviceMeasurementsViewModel
    {
        [Required(ErrorMessage = "Height is required.")]
        public decimal DimensionsHeight { get; set; }
        [Required(ErrorMessage = "Length is required.")]
        public decimal DimensionsLength { get; set; }
        [Required(ErrorMessage = "Width is required.")]
        public decimal DimensionsWidth { get; set; }
        [Required(ErrorMessage = "Weight is required.")]
        public decimal WeightInKilograms { get; set; }

        public bool IsAllDimensionsSet => DimensionsLength != 0 || DimensionsHeight != 0 || DimensionsWidth != 0;
    }
}