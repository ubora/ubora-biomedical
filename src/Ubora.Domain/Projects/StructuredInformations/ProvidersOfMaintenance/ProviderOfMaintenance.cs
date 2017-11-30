using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public abstract class ProviderOfMaintenance
    {
        public abstract string Key { get; }
        public abstract string ToDisplayName();

        public static IReadOnlyDictionary<string, Type> ProviderOfMaintenanceKeyTypeMap = new Dictionary<string, Type>
        {
            {"empty", typeof(EmptyProviderOfMaintenance)},
            {"other", typeof(OtherProviderOfMaintenance)},
            {"self-user_or_patient", typeof(SelfUserOrPatient)},
            {"nurse_or_physician", typeof(NurseOrPhysician)},
            {"engineer", typeof(Engineer)},
            {"manufacturer", typeof(Manufacturer)},
            {"technician", typeof(Technician)},
        };

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProviderOfMaintenance)obj);
        }

        protected bool Equals(ProviderOfMaintenance other)
        {
            return string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Key != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Key) : 0);
        }
    }
}
