using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ubora.Web.Tests.Helper
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

            instance.GetType()
                .GetProperty(memberExpression.Member.Name, AllInstanceBindingFlags)
                .SetValue(instance, value);

            return instance;
        }

        public static T Set<T, P>(this T instance, string propertyName, P value) where T : class
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException(nameof(string.IsNullOrWhiteSpace), nameof(propertyName));
            }

            instance.GetType()
                .GetProperty(propertyName, AllInstanceBindingFlags)
                .SetValue(instance, value);

            return instance;
        }

        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            var propertyValue = instance.GetType()
                .GetProperty(propertyName, AllInstanceBindingFlags)
                .GetValue(instance);

            return (T) propertyValue;
        }

        public static object InvokeMethod<T>(this T instance, string methodName, params object[] parameters)
        {
            var methodInfo = instance.GetType()
                .GetMethod(methodName, AllInstanceBindingFlags);

            return methodInfo.Invoke(instance, parameters);
        }
    }
}
