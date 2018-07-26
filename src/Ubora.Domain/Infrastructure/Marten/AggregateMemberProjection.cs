using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Marten.Storage;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure.Marten
{
    internal class AggregateMemberProjection<T, TEvent> : AggregateMemberProjectionBase<T, TEvent>
        where T : class, new()
        where TEvent : class, IAggregateMemberEvent
    {
        protected override T LoadAggregate(IDocumentSession session, TEvent @event)
        {
            var aggregateMemberId = @event.Id;
            return session.Load<T>(aggregateMemberId) ?? new T();
        }
    }

    internal abstract class AggregateMemberProjectionBase<T, TEvent> 
        : IProjection 
        where T : class, new() 
        where TEvent : class
    {
        public Type[] Consumes => new[] { typeof(TEvent) };

        public Type Produces => typeof(T);

        public AsyncOptions AsyncOptions => new AsyncOptions();

        public void Apply(IDocumentSession session, EventPage page)
        {
            foreach (var stream in page.Streams)
            {
                foreach (var martenEvent in stream.Events)
                {
                    var @event = martenEvent.Data as TEvent;
                    if (@event == null)
                    {
                        continue;
                    }

                    var aggregateMember = LoadAggregate(session, @event);

                    var eventApplyMethod = typeof(T)
                        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Single(method => IsApplyMethodForType(method, @event.GetType()));

                    // TODO: Apply methods with IEvent<> are not yet supported.
                    eventApplyMethod.Invoke(aggregateMember, new object[] { @event });

                    session.Store(aggregateMember);
                }
            }
        }

        protected abstract T LoadAggregate(IDocumentSession session, TEvent @event);

        private static bool IsApplyMethodForType(MethodInfo methodInfo, Type type)
        {
            var methodParameters = methodInfo.GetParameters();
            var isApplyMethod = (methodInfo.Name == "Apply" && methodParameters.Length == 1);

            if (!isApplyMethod)
            {
                return false;
            }

            return methodParameters.Single().ParameterType == type;
        }

        public Task ApplyAsync(IDocumentSession session, EventPage page, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void EnsureStorageExists(ITenant tenant)
        {
            throw new NotImplementedException();
        }
    }
}