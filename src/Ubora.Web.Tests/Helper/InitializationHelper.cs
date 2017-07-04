using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ubora.Web.Tests.Helper
{
    public static class InitializationHelper
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

            typeof(T)
                .GetProperty(memberExpression.Member.Name, AllInstanceBindingFlags)
                .SetValue(instance, value);

            return instance;
        }

        public static T SetPropertyValue<T>(this T instance, string propertyName, object newValue)
        {
            var type = instance.GetType();

            var prop = type.GetProperty(propertyName, AllInstanceBindingFlags);

            prop.SetValue(instance, newValue, null);

            return instance;
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName, AllInstanceBindingFlags).GetValue(obj);
        }

        public static object InvokeMethod<T>(this T obj, string methodName, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(methodName, AllInstanceBindingFlags);

            return methodInfo.Invoke(obj, parameters);
        }

    }
}
