using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;

namespace Ubora.Domain.Projects
{
    public class WorkpackagesProjection : IProjection
    {
        public Type[] Consumes => new[] { typeof(WorkpackageCreatedEvent) };

        public Type Produces => typeof(Workpackage);

        public AsyncOptions AsyncOptions => new AsyncOptions();

        public void Apply(IDocumentSession session, EventStream[] streams)
        {
            foreach (var stream in streams)
            {
                foreach (var @event in stream.Events)
                {
                    if (!(@event.Data is WorkpackageCreatedEvent))
                        continue;
                    var workpackageEvent = (WorkpackageCreatedEvent)@event.Data;
                    var aggregate = session.Load<Workpackage>((Guid) workpackageEvent.Id) ?? new Workpackage();
                    aggregate.Apply((dynamic)@event);
                    session.Store(aggregate);
                }
            }

        }

        public Task ApplyAsync(IDocumentSession session, EventStream[] streams, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
