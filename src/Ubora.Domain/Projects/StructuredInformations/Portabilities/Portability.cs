using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects.StructuredInformations.Portabilities
{
    public abstract class Portability
    {
        public abstract string Key { get; }
        public abstract string ToDisplayName();

        public static IReadOnlyDictionary<string, Type> PortabilityKeyTypeMap = new Dictionary<string, Type>
        {
            {"empty", typeof(EmptyPortability)},
            {"installed_and_stationary", typeof(InstalledAndStationary)},
            {"mobile", typeof(Mobile)},
            {"portable", typeof(Portable)}
        };

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Portability)obj);
        }

        protected bool Equals(Portability other)
        {
            return string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Key != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Key) : 0);
        }
    }
}
