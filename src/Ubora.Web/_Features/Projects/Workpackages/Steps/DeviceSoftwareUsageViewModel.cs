using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class DeviceSoftwareUsageViewModel
    {
        [Required]
        public bool DoesItUseAnyKindOfSoftware { get; set; }
        [RequiredIf(nameof(DoesItUseAnyKindOfSoftware), true)]
        public string IfUsesSoftwareDescribeSoftware { get; set; }
        [RequiredIf(nameof(DoesItUseAnyKindOfSoftware), true)]
        public string IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse { get; set; }
    }
}