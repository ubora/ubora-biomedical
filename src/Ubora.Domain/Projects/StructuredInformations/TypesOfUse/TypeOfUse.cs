using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects.StructuredInformations.TypesOfUse
{
    public abstract class TypeOfUse
    {
        public abstract string Key { get; }
        public abstract string ToDisplayName();

        public static IReadOnlyDictionary<string, Type> TypeOfUseKeyTypeMap = new Dictionary<string, Type>
        {
            {"empty", typeof(EmptyTypeOfUse)},
            {"single_use", typeof(SingleUse)},
            {"long_term_use", typeof(LongTermUse)},
            {"reusable", typeof(Reusable)},
            {"capital_equipment", typeof(CapitalEquipment)},
        };

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeOfUse)obj);
        }

        protected bool Equals(TypeOfUse other)
        {
            return string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Key != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Key) : 0);
        }
    }
}
