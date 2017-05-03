using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;

namespace Ubora.Domain.Projects
{
    public class AggregateMemberProjection<TResult, TEvent> : IProjection where TResult : class, new()
    {
        public Type[] Consumes => new[] { typeof(TEvent) };

        public Type Produces => typeof(TResult);

        public AsyncOptions AsyncOptions => new AsyncOptions();

        public void Apply(IDocumentSession session, EventStream[] streams)
        {
            foreach (var stream in streams)
            {
                foreach (var @event in stream.Events)
                {
                    if (!(@event.Data is TEvent))
                        continue;

                    var eventData = (TEvent)@event.Data;

                    var id = ((dynamic)eventData).Id;
                    var aggregate = session.Load<TResult>((Guid)id) ?? new TResult();

                    var method = typeof(TResult)
                        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Single(x => x.Name == "Apply" && x.GetParameters().Length == 1 && x.GetParameters().Single().ParameterType == @event.Data.GetType());

                    method.Invoke(aggregate, new [] { @event.Data });

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