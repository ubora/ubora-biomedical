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

        public bool DoesItUseAnyKindOfSoftware { get; private set; }
        public string IfUsesSoftwareDescribeSoftware { get; private set; }
        public string IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse { get; private set; }

        public static DeviceSoftwareUsage CreateSoftwareIsUsed(string description, string localUseDescription)
        {
            return new DeviceSoftwareUsage
            {
                DoesItUseAnyKindOfSoftware = true,
                IfUsesSoftwareDescribeSoftware = description,
                IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse = localUseDescription
            };
        }

        public static DeviceSoftwareUsage CreateSoftwareIsNotUsed()
        {
            return new DeviceSoftwareUsage
            {
                DoesItUseAnyKindOfSoftware = false
            };
        }

        public static DeviceSoftwareUsage CreateEmpty()
        {
            return new DeviceSoftwareUsage();
        }
    }
}