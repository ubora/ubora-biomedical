using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class DeviceSoftwareUsage
    {
        public DeviceSoftwareUsage(bool doesItUseAnyKindOfSoftware, string ifUsesSoftwareDescribeSoftware, string ifUsesSoftwareCanSoftwareBeCustomizedForLocalUse)
        {
            DoesItUseAnyKindOfSoftware = doesItUseAnyKindOfSoftware;
            IfUsesSoftwareDescribeSoftware = ifUsesSoftwareDescribeSoftware;
            IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = ifUsesSoftwareCanSoftwareBeCustomizedForLocalUse;
        }

        [JsonConstructor]
        public DeviceSoftwareUsage()
        {
        }

        public bool DoesItUseAnyKindOfSoftware { get; set; }
        public string IfUsesSoftwareDescribeSoftware { get; set; }
        public string IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse { get; set; }

        public static DeviceSoftwareUsage CreateEmpty()
        {
            return new DeviceSoftwareUsage();
        }
    }
}