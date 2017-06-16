using System.Reflection;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Tests
{
    public static class EntityExtensions
    {
        // Test-helper method for invoking the private "Apply" method of event sourced aggregates.
        public static void Apply<TAggregate, TEvent>(this Entity<TAggregate> entity, TEvent @event)
            where TAggregate : Entity<TAggregate>
        {
            var entityType = entity.GetType();

            var applyMethod = entityType.GetMethod("Apply", BindingFlags.NonPublic);

            applyMethod.Invoke(entity, new object[] { @event });
        }
    }
}
