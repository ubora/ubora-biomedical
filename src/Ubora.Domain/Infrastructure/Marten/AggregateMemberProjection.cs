using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure.Marten
{
    internal class AggregateMemberProjection<T, TEvent> : IProjection where T : class, new() where TEvent : class, IAggregateMemberEvent
    {
        public Type[] Consumes => new[] { typeof(TEvent) };

        public Type Produces => typeof(T);

        public AsyncOptions AsyncOptions => new AsyncOptions();

        public void Apply(IDocumentSession session, EventStream[] streams)
        {
            foreach (var stream in streams)
            {
                foreach (var martenEvent in stream.Events)
                {
                    var @event = martenEvent.Data as TEvent;
                    if (@event == null)
                    {
                        continue;
                    }

                    var aggregateMemberId = @event.Id;
                    var aggregateMember = session.Load<T>(aggregateMemberId) ?? new T();

                    var eventApplyMethod = typeof(T)
                        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Single(method => IsApplyMethodForType(method, @event.GetType()));

                    // TODO: Apply methods with IEvent<> are not yet supported.
                    eventApplyMethod.Invoke(aggregateMember, new object[] { @event });

                    session.Store(aggregateMember);
                }
            }
        }

        private static bool IsApplyMethodForType(MethodInfo methodInfo, Type type)
        {
            var methodParameters = methodInfo.GetParameters();
            var isApplyMethod = methodInfo.Name == "Apply" && methodParameters.Length == 1;

            if (!isApplyMethod)
            {
                return false;
            }

            return methodParameters.Single().ParameterType == type;
        }

        public Task ApplyAsync(IDocumentSession session, EventStream[] streams, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}