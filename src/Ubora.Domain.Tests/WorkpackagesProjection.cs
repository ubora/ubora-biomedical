using Marten;
using Marten.Events.Projections;
using System;
using Marten.Events;
using System.Threading;
using System.Threading.Tasks;
using Marten.Events.Projections.Async;
using Ubora.Domain.Projects;

namespace Ubora.Domain.Tests
{
    public partial class UnitTest1
    {
        public class WorkpackagesProjection : IProjection
        {
            public Type[] Consumes => new [] { typeof(WorkpackageCreated) };

            public Type Produces => typeof(Workpackage);

            public AsyncOptions AsyncOptions => new AsyncOptions();

            public void Apply(IDocumentSession session, EventStream[] streams)
            {
                foreach(var stream in streams)
                {
                    foreach(var @event in stream.Events)
                    {
                        if (!(@event.Data is WorkpackageCreated))
                            continue;
                        var workpackageEvent = (WorkpackageCreated)@event.Data;
                        var aggregate = session.Load<Workpackage>(workpackageEvent.Id) ?? new Workpackage();
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
}
