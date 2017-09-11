using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class ReflectionHelper
    {
        private const BindingFlags AllInstanceBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static T Set<T, P>(this T instance, Expression<Func<T, P>> propertySelector, P value) where T : class
        {
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new MemberAccessException();
            }
            var memberExpression = (MemberExpression)propertySelector.Body;

            SetPropertyValue(instance, value, memberExpression.Member.Name);

            return instance;
        }

        public static T Set<T, P>(this T instance, string propertyName, P value) where T : class
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException(nameof(string.IsNullOrWhiteSpace), nameof(propertyName));
            }

            SetPropertyValue(instance, value, propertyName);

            return instance;
        }

        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            var propertyValue = instance.GetType().GetProperty(propertyName, AllInstanceBindingFlags)
                .GetValue(instance);

            return (T)propertyValue;
        }

        public static object InvokeMethod<T>(this T instance, string methodName, params object[] parameters)
        {
            var methodInfo = instance.GetType().GetMethod(methodName, AllInstanceBindingFlags);

            return methodInfo.Invoke(instance, parameters);
        }

        private static void SetPropertyValue<T, P>(T instance, P value, string propertyName) where T : class
        {
            var setMethod = FindPropertySetMethod(instance.GetType(), propertyName);

            setMethod.Invoke(instance, new object[] {value });
        }

        private static MethodInfo FindPropertySetMethod(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName, AllInstanceBindingFlags);

            if (propertyInfo.SetMethod != null)
            {
                return propertyInfo.SetMethod;
            }

            // Look for set method from base type.
            var baseType = type.GetTypeInfo().BaseType;
            if (baseType == typeof(object))
            {
                throw new InvalidOperationException("Property set method could not be found.");
            }

            return FindPropertySetMethod(baseType, propertyName);
        }
    }
}
