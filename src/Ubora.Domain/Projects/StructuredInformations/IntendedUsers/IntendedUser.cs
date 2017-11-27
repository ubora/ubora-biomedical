using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public abstract class IntendedUser
    {
        public abstract string Key { get; }
        public abstract string ToDisplayName();

        public static IReadOnlyDictionary<string, Type> IntendedUserKeyTypeMap = new Dictionary<string, Type>
        {
            {"self-use_or_patient", typeof(SelfUseOrPatient)},
            {"physician", typeof(Physician)},
            {"technician", typeof(Technician)},
            {"nurse", typeof(Nurse)},
            {"midwife", typeof(Midwife)},
            {"family_member", typeof(FamilyMember)},
            {"other", typeof(Other)},
        };

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntendedUser)obj);
        }

        protected bool Equals(IntendedUser other)
        {
            return string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Key != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Key) : 0);
        }
    }
}
