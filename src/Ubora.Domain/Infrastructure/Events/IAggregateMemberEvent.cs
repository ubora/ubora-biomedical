using System;

namespace Ubora.Domain.Infrastructure.Events
{
    public interface IAggregateMemberEvent
    {
        Guid Id { get; }
    }
}