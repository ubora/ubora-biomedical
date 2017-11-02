using Marten.Events;
using Marten.Events.Projections;
using System;

namespace Ubora.Web.Tests.Helper
{
    public class TestEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long Sequence { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object Data { get; set; }

        public Guid StreamId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string StreamKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset Timestamp { get; set; }
        public string TenantId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Apply<TAggregate>(TAggregate state, IAggregator<TAggregate> aggregator) where TAggregate : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
