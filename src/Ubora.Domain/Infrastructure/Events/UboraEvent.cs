using System;

namespace Ubora.Domain.Infrastructure.Events
{
    /// <remarks> Please name concrete events in past tense. </remarks>>
    public abstract class UboraEvent
    {
        protected UboraEvent(UserInfo initiatedBy)
        {
            InitiatedBy = initiatedBy ?? throw new ArgumentNullException(nameof(initiatedBy));
            Timestamp = DateTime.UtcNow;
        }

        public UserInfo InitiatedBy { get; }

        public DateTime Timestamp { get; private set; }

        public abstract string GetDescription();

        public override string ToString()
        {
            return $"{StringTokens.User(InitiatedBy.UserId)} {GetDescription()}";
        }
    }
}
