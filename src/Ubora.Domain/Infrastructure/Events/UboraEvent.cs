using System;

namespace Ubora.Domain.Infrastructure.Events
{
    public abstract class UboraEvent
    {
        protected UboraEvent(UserInfo initiatedBy)
        {
            InitiatedBy = initiatedBy ?? throw new ArgumentNullException(nameof(initiatedBy));
        }

        public UserInfo InitiatedBy { get; }

        public abstract string GetDescription();

        public override string ToString()
        {
            return $"\"{InitiatedBy.Name}\": {GetDescription()}";
        }
    }
}
