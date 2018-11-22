using System;

namespace Ubora.Domain.Infrastructure.Events
{
    /// <remarks> Please name concrete events in past tense. </remarks>>
    public abstract class UboraEvent
    {
        protected UboraEvent(UserInfo initiatedBy)
        {
            InitiatedBy = initiatedBy ?? throw new ArgumentNullException(nameof(initiatedBy));
            Timestamp = DateTimeOffset.UtcNow;
        }

        public UserInfo InitiatedBy { get; }

        public DateTimeOffset Timestamp { get; internal set; }

        public abstract string GetDescription();

        public override string ToString()
        {
            return $"{StringTokens.User(InitiatedBy.UserId)} {GetDescription()}";
        }
    }
}
