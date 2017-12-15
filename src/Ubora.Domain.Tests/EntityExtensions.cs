using System;
using System.Linq;
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

            // Look for method from base type when dynamically mocked object.
            if (entityType.Namespace == "Castle.Proxies")
            {
                entityType = entityType.GetTypeInfo().BaseType;
            }

            var applyMethod = FindApplyMethod<TAggregate, TEvent>(entityType);

            try
            {
                applyMethod.Invoke(entity, new object[] { @event });
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException != null)
                {
                    throw e.InnerException;
                }
                throw;
            }
        }

        private static MethodInfo FindApplyMethod<TAggregate, TEvent>(Type entityType) where TAggregate : Entity<TAggregate>
        {
            var applyMethod = entityType
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .SingleOrDefault(method => IsApplyMethodForType(method, typeof(TEvent)));

            if (applyMethod != null)
            {
                return applyMethod;
            }

            if (entityType.BaseType == typeof(object))
            {
                throw new InvalidOperationException($"Apply method could not be found for aggregate type: {nameof(TAggregate)} and event type: {nameof(TEvent)}");
            }
            return FindApplyMethod<TAggregate, TEvent>(entityType.BaseType);
        }
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
    }
}
